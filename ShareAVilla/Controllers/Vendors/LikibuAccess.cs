using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace ShareAVilla.Controllers.Likibu
{
        
    public static class LikibuAccess
    {
        private const string ApiKey = "?key=8v9B2P887ahW9gaP";
        private const string DestinationRqString = "https://api.likibu.com/destinations/search";
        private const string DestinationDetailRqString = "https://api.likibu.com/destinations/";
        private const string SearchSessionString = "https://api.likibu.com/search/";
        private const string PingString = "https://api.likibu.com/ping/";
        private const string DetailsRqString = "https://api.likibu.com/rooms/";
        private static WebClient webClient = new WebClient();
        public static int SearchTimeoutInSec = 60;

        public enum ResponseCode
        {
            OK = 200,
            SessionCreated = 201,
            Unauthorized = 401,
            Forbidden = 402,
            NotFound = 404,
            Error = 500
        }


        //To request possible destinations from substrings
        public static async Task<List<Models.Accommodation.Likibu.Destination>> DestinationRequest(string destination, string lang, string numSuggestions)
        {
            try
            {
                //
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                string webrq = DestinationRqString + ApiKey + "&q=" + destination + "&culture=" + lang + "&size=" + numSuggestions;
                
                string ret = await webClient.DownloadStringTaskAsync(webrq);
                if (ret == null)
                {
                    NotificationManager.AddException(new Exception("Likibu Destination Request returned null"));
                    return null;
                }
                List<Models.Accommodation.Likibu.Destination> dest = ParseJSON4Destinations(ret);
                return dest;
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
            }
            return null;

        }
        private static List<Models.Accommodation.Likibu.Destination> ParseJSON4Destinations(string js)
        {
            var jo = JObject.Parse(js);
            List<Models.Accommodation.Likibu.Destination> dest = new List<Models.Accommodation.Likibu.Destination>();
            int num = jo["results"].Count();
            for (int i = 0; i < num; i++)
            {
                string id = jo["results"][i]["id"].ToString();
                string name = jo["results"][i]["name"].ToString();
                string pname = jo["results"][i]["parent_name"].ToString();
                int numRes = Int32.Parse(jo["results"][i]["results"].ToString());
                string ccode = jo["results"][i]["country_code"].ToString();
                Models.Accommodation.Likibu.Destination d = new Models.Accommodation.Likibu.Destination(id, name, pname, numRes, ccode);
                dest.Add(d);
            }

            return dest;
        }

        public static async Task<string> SendPing()
        {
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            string webrq = PingString + ApiKey;
            string ret = await webClient.DownloadStringTaskAsync(webrq);
            return ret;
        }
        
        public static string ConvertDateString(string date)
        {
            DateTime DT = DateTime.Parse(date);
            string Date = DateTime.Parse(DT.ToShortDateString()).ToString("yyyy-MM-dd");
            return Date;
        }
      
        public static async Task<string> StartSearchSession(Models.Find.SearchVM searchVM)
        {
            try
            {

                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string webrq = SearchSessionString + ApiKey;
               
                Models.Accommodation.Likibu.StartSearchSessionBody Param = new Models.Accommodation.Likibu.StartSearchSessionBody(searchVM);                                
                string retStr = await webClient.UploadStringTaskAsync(webrq, "POST", Param.Body);   
                Models.Accommodation.Likibu.StartSearchSessionReturnObject SssRObj = ParseSSSResponse(retStr);
                return SssRObj.ID;
            }
            catch (WebException e)
            {
                NotificationManager.AddException(e);
            }
            return null;
        }
        private static Models.Accommodation.Likibu.StartSearchSessionReturnObject ParseSSSResponse(string ret)
        {
            JObject jo = JObject.Parse(ret);
            Models.Accommodation.Likibu.StartSearchSessionReturnObject retObj = new Models.Accommodation.Likibu.StartSearchSessionReturnObject(jo["search_id"].ToString(), jo["search_status_url"].ToString(), jo["search_results_url"].ToString());
            return retObj;
        }


        public static async Task<Models.Accommodation.Likibu.PollSearchSessionResponse> PollSearchStatus(string SearchID)
        {
            
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string webrq = SearchSessionString + SearchID + "/status" + ApiKey;

            string ret = await webClient.DownloadStringTaskAsync(webrq);
            return ParseSearchStatusResponse(ret);
            
        }
        private static Models.Accommodation.Likibu.SearchRequestResults ParseSearchResultResponse(string Response)
        {
            JObject jo = JObject.Parse(Response);
            Models.Accommodation.Likibu.SearchRequestResults r = new Models.Accommodation.Likibu.SearchRequestResults();
            r.SearchID = jo["search_id"].ToString();
            r.Status = jo["status"].ToString();
            r.Bbox = jo["bbox"].ToString();
            r.Where = jo["where"].ToString();
            r.CheckIn = jo["checkin"].ToString();
            r.CheckOut = jo["checkout"].ToString();
            r.CountryCode = jo["country_code"].ToString();
            r.TotalResults = Int32.Parse(jo["total_results"].ToString());
            r.TotalPages = Int32.Parse(jo["total_pages"].ToString());
            r.MinPrice = Int32.Parse(jo["min_price"].ToString());
            r.MaxPrice = Int32.Parse(jo["max_price"].ToString());
            r.Offers = ParseOffers(jo);
            //Todo read Facets
            return r;
        }
     
        private static List<Models.Accommodation.Likibu.Offer> ParseOffers(JObject jo)
        {
           
            int num = jo["offers"].Count();
            List<Models.Accommodation.Likibu.Offer> r = new List<Models.Accommodation.Likibu.Offer>();
            for (int i = 0; i < num; i++)
            {
                Models.Accommodation.Likibu.Offer o = ParseOfferJson(jo, i);                
                r.Add(o);
            }
            return r;
        }
        private static Models.Accommodation.Likibu.PollSearchSessionResponse ParseSearchStatusResponse(string retString)
        {
            var jo = JObject.Parse(retString);
            string searchid = jo["search_id"].ToString();
            string where = jo["where"].ToString();
            string country = jo["country_code"].ToString();
            string bbox = jo["bbox"].ToString();
            string partners = jo["status"]["partners"].ToString();
            string status = jo["status"]["is_complete"].ToString();

            Models.Accommodation.Likibu.PollSearchSessionResponse ret = new Models.Accommodation.Likibu.PollSearchSessionResponse(searchid, where, country, bbox, status, partners);
            return ret;

        }
        private static Models.Accommodation.Likibu.Offer ParseOfferJson(JObject jo, int i)
        {
            Models.Accommodation.Likibu.Offer o = new Models.Accommodation.Likibu.Offer();
            //Add Thumbnails
            int count = (jo["offers"][i]["thumbnails"].Count());
            o.Thumbnails = new List<string>();
            for (int j = 0; j < count; j++)
            {
                o.Thumbnails.Add(jo["offers"][i]["thumbnails"][j].ToString());                
            }
            
            //Add Photos
            count = (jo["offers"][i]["photos"].Count());
            o.Photos = new List<string>();
            for (int j = 0; j < count; j++)
            {
                o.Photos.Add(jo["offers"][i]["photos"][j].ToString());
            }
            
            o.Amenities = jo["offers"][i]["amenities"].ToString();            
            o.offerID = jo["offers"][i]["id"].ToString();
            o.Title = jo["offers"][i]["title"].ToString();
            o.Description = jo["offers"][i]["description"].ToString();
            o.URL = jo["offers"][i]["url"].ToString();
            o.Title = jo["offers"][i]["title"].ToString();
            o.PriceTotal = float.Parse(jo["offers"][i]["price_sojourn"].ToString());
            o.PriceNight = float.Parse(jo["offers"][i]["price"].ToString());            
            o.Currency = jo["offers"][i]["currency"].ToString();
            o.Lat = float.Parse(jo["offers"][i]["lat"].ToString());
            o.Lng = float.Parse(jo["offers"][i]["lng"].ToString());
            o.AverageRating = float.Parse(jo["offers"][i]["rating"].ToString());
            o.RatingCount = int.Parse(jo["offers"][i]["rating_count"].ToString());
            o.MaxGuests = int.Parse(jo["offers"][i]["max_guests"].ToString());
            o.BedRooms = int.Parse(jo["offers"][i]["bedrooms"].ToString());
            o.BathRooms = int.Parse(jo["offers"][i]["bathrooms"].ToString());
            o.SurfaceSquareMeters = jo["offers"][i]["surface"].ToString();
            o.PrivacyType = jo["offers"][i]["privacy_type"].ToString();
            o.Type = jo["offers"][i]["type"].ToString();
            o.SourceID = jo["offers"][i]["source"]["id"].ToString();
            o.SourceName = jo["offers"][i]["source"]["name"].ToString();
            o.SourceSlug = jo["offers"][i]["source"]["slug"].ToString();
            o.IsInstantBooking = bool.Parse(jo["offers"][i]["is_instant_booking_available"].ToString());
            o.Destination = jo["offers"][i]["destination"].ToString();
            return o;
        }
        public static async Task<Models.Accommodation.Likibu.SearchRequestResults> RequestSearchResults(Models.Find.SearchVM searchVM)
        {
            
            string webrq = SearchSessionString + searchVM.Search.LikibuSessionID + ApiKey;
            Models.Accommodation.Likibu.SearchResultsRequestBody SRP = new Models.Accommodation.Likibu.SearchResultsRequestBody(searchVM.Search.FilterProps.PerPage, searchVM.Search.FilterProps.Page, searchVM.Search.FilterProps.Sort, searchVM.Search.FilterProps.PrivacyType, searchVM.Search.FilterProps.AcomType, searchVM.Search.FilterProps.Amenities, searchVM.Search.FilterProps.PartnerID, searchVM.Search.BedRooms.ToString(), searchVM.Search.FilterProps.PriceMin, searchVM.Search.FilterProps.PriceMax);
            string retStr = await webClient.DownloadStringTaskAsync(webrq+SRP.Body);

            Models.Accommodation.Likibu.SearchRequestResults resultsFinal = new Models.Accommodation.Likibu.SearchRequestResults();
            resultsFinal = ParseSearchResultResponse(retStr);
            return resultsFinal;

        }
       

        public static Models.Accommodation.Likibu.AccomodationDetails ParseDetailsResult(string res)
        {
            Models.Accommodation.Likibu.AccomodationDetails details = new Models.Accommodation.Likibu.AccomodationDetails();
            details.offer = new Models.Accommodation.Likibu.Offer();
            JObject jo = JObject.Parse(res);
            details.offer.offerID = jo["id"].ToString();
            details.offer.SourceID = jo["source"]["id"].ToString();
            
            details.offer.SourceName = jo["source"]["name"].ToString();
            details.offer.SourceSlug = jo["source"]["slug"].ToString();
            details.offer.GenTitle = jo["generated_title"].ToString();
            details.offer.Title = jo["title"].ToString();
            details.offer.Description = jo["description"].ToString();
            details.offer.Type = jo["type"].ToString();
            details.offer.PrivacyType = jo["privacy_type"].ToString();

            details.offer.Photos = new List<string>();
            int count = jo["photos"].Count();
            for (int j = 0; j < count; j++)
            {
                details.offer.Photos.Add(jo["photos"][j].ToString());
            }
             
            ///Add Ammenities            
            details.offer.Amenities = jo["place"]["amenities"].ToString();
            details.offer.URL = jo["url"].ToString();
            details.offer.PriceTotal = float.Parse(jo["price_sojourn"].ToString());
            details.offer.PriceNight = float.Parse(jo["price_nightly"].ToString());

            details.offer.PriceWeek = float.Parse(jo["price_weekly"].ToString());
            details.offer.PriceMonth = float.Parse(jo["price_monthly"].ToString());
            details.offer.Currency = jo["currency"].ToString();

            details.offer.Lat = float.Parse(jo["location"]["location"][1].ToString());
            details.offer.Lng = float.Parse(jo["location"]["location"][0].ToString());
            
            count = (jo["location"]["destination_id"].Count());
            string[] destIds = new string[count];
            for (int i = 0; i < count; i++)
            {
                destIds[i] = jo["location"]["destination_id"][i].ToString();                
            }
            //details.offer.Destination = jo["location"]["destination"].ToString();
            details.offer.DestinationIDs = Models.Accommodation.Likibu.Offer.ArrayToString(destIds);
            details.offer.City = jo["location"]["city"].ToString();
            details.offer.Country = jo["location"]["country"].ToString();
            details.offer.AverageRating = float.Parse(jo["rating"].ToString());
            details.offer.RatingCount = int.Parse(jo["rating_count"].ToString());
            details.offer.MaxGuests = int.Parse(jo["booking"]["max_guests"].ToString());
            details.offer.BedRooms = int.Parse(jo["place"]["bedrooms"].ToString());
          //  details.offer.Rooms = jo["place"]["rooms"].ToString(); does not exist!
            details.offer.BathRooms = int.Parse(jo["place"]["bathrooms"].ToString());
            details.offer.SurfaceSquareMeters = jo["place"]["surface"].ToString();
            details.offer.IsInstantBooking = Boolean.Parse(jo["booking"]["is_instant_booking_available"].ToString());
            details.offer.MinNights = int.Parse(jo["booking"]["min_nights"].ToString());
            details.offer.MaxNights = int.Parse(jo["booking"]["max_nights"].ToString());
            return details;
        }
       
        public static async Task<Models.Accommodation.Likibu.AccomodationDetails> RequestAccomodationDetails(Models.Accommodation.Likibu.AccomodationDetailsParam para, string roomID)
        {
            try
            {
                string webrq = DetailsRqString + roomID + ApiKey;
                string rqString = webrq + para.Body;
                string retStr = null;
                retStr = await webClient.DownloadStringTaskAsync(rqString);
                Models.Accommodation.Likibu.AccomodationDetails ADetails = ParseDetailsResult(retStr);
                return ADetails;
            }
            catch(Exception e)
            {
                NotificationManager.AddException(e);
                return null;
            }
                
        }

    }
}
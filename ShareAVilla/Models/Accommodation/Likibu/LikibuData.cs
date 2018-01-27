using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.Accommodation.Likibu
{

    public static class AccomType
    {
        public const string Apartment = "apartment";
        public const string House = "house";
        public const string Villa = "villa";
        public const string Lodge = "lodge";
        public const string Castle = "castle";
        public const string Caravan = "caravan";
        public const string Resort = "resort";
    }
    public class Amenities
    {
        private const string AirCon = "ac";
        private const string WheelAccess = "accessible";
        private const string Balcony = "balcony";
        private const string Breakfast = "breakfast";
        private const string Dishwasher = "dishwasher";
        private const string Elevator = "elevator";
        private const string Events = "events";
        private const string Family = "family";
        private const string Gym = "gym";
        private const string Fireplace = "fireplace";
        private const string Jacuzzi = "jacuzzi";
        private const string Kitchen = "kitchen";
        private const string Wifi = "net";
        private const string Parking = "parking";
        private const string Pets = "pets";
        private const string Pool = "pool";
        private const string Smoke = "smoke";
        private const string TV = "tv";
        private const string WashDry = "washer_dryer";
        private string AmenitiesList = "";
        public string GenerateAmenitiesString(bool ac, bool wheel, bool balc, bool bfast, bool dishw, bool elev, bool events, bool fam, bool gym, bool fire, bool jacu, bool kitch, bool wifi, bool park, bool pet, bool pool, bool smok, bool tv, bool washdry)
        {
            if (ac) AmenitiesList += AirCon + ",";
            if (wheel) AmenitiesList += WheelAccess + ",";
            if (balc) AmenitiesList += Balcony + ",";
            if (bfast) AmenitiesList += Breakfast + ",";
            if (dishw) AmenitiesList += Dishwasher + ",";
            if (elev) AmenitiesList += Elevator + ",";
            if (events) AmenitiesList += Events + ",";
            if (fam) AmenitiesList += Family + ",";
            if (gym) AmenitiesList += Gym + ",";
            if (fire) AmenitiesList += Fireplace + ",";
            if (jacu) AmenitiesList += Jacuzzi + ",";
            if (kitch) AmenitiesList += Kitchen + ",";
            if (wifi) AmenitiesList += Wifi + ",";
            if (park) AmenitiesList += Parking + ",";
            if (pet) AmenitiesList += Pets + ",";
            if (pool) AmenitiesList += Pool + ",";
            if (smok) AmenitiesList += Smoke + ",";
            if (tv) AmenitiesList += TV + ",";
            if (washdry) AmenitiesList += WashDry;
            return AmenitiesList;

        }
        public string GetAmenities() { return AmenitiesList; }
    }
    public static class Sort
    {
        public const string Reco = "reco";
        public const string PriceAsc = "price_asc";
        public const string PriceDesc = "price_desc";
    }

    public class Destination
    {
        public string ID;
        public string Name;
        public string ParentName;
        public int NumResults;
        public string CountryCode;
        public Destination(string id, string name, string pname, int numresults, string ccode)
        {
            ID = id;
            Name = name;
            ParentName = pname;
            NumResults = numresults;
            CountryCode = ccode;
        }
    }
    /// <summary>
    /// Get ChildIDs for Locationing
    /// </summary>
    public class DestinationDetail
    {
        public string ID { get; set; }
        public int Results { get; set; }
        public string Name { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }

    }
    public class Offer:BaseDataObject
    {
        
        
       
        [Display(Name = "Title: ")]
        public string Title { get; set; }
        [Display(Name = "Generated Title: ")]
        public string GenTitle { get; set; }
        [Display(Name = "Description: ")]
        public string Description { get; set; }
        [Display(Name = "Destination: ")]
        public string Destination { get; set; }
        [Display(Name = "Link: ")]
        public string URL { get; set; }
        public List<string> Thumbnails { get; set; }
        public List<string> Photos { get; set; }
        [Display(Name = "Total Price: ")]
        public float PriceTotal { get; set; }
        [Display(Name = "Price per night: ")]
        public float PriceNight { get; set; }
        [Display(Name = "Price per week: ")]
        public float PriceWeek { get; set; }
        [Display(Name = "Price per month: ")]
        public float PriceMonth { get; set; }
        public string offerID { get; set; }
        public string Currency { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DestinationIDs { get; set; }
        [Display(Name = "Rating: ")]
        public float AverageRating { get; set; }

        public int RatingCount { get; set; }
        public int MaxGuests { get; set; }
        [Display(Name = "Rooms: ")]
        public int Rooms { get; set; }
        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }
        [Display(Name = "Bathrooms: ")]
        public int BathRooms { get; set; }
        public bool IsInstantBooking { get; set; }
        [Display(Name = "sq m: ")]
        public string SurfaceSquareMeters { get; set; }
        [Display(Name = "Amenities: ")]
        public string Amenities { get; set; }
        [Display(Name = "Privacy: ")]
        public string PrivacyType { get; set; }
        [Display(Name = "Accomodation: ")]
        public string Type { get; set; }
        public string SourceID { get; set; }
        public string SourceOfferID { get; set; }
        public string SourceSlug { get; set; }
        public string SourceName { get; set; }
        [Display(Name = "Min nights: ")]
        public int MinNights { get; set; }
        [Display(Name = "Max nights: ")]
        public int MaxNights { get; set; }
        public static string ArrayToString(string[] array)
        {
            string res="";
            for (int i = 0; i < array.Length; i++)
            {
                if (i == array.Length - 1)
                {
                    res = res + array[i].ToString();
                }
                else
                {
                    res = res + array[i].ToString() + "@";
                }
            }
            return res;
        }
        public static string[] StringToArray(string str)
        {
            string[] res = str.Split('@');
            
            return res;
        }
    }
    
    public class StartSearchSessionBody
    {

        private string where;    
        private string checkin;
        private string checkout;
        private string guests;
        private string culture;
        private string currency;          
        public string Body { get; }
        public StartSearchSessionBody(Find.SearchVM searchVM)
        {
            where = searchVM.Search.Location;
            checkin = Controllers.Likibu.LikibuAccess.ConvertDateString(searchVM.Search.CheckIn);
            checkout = Controllers.Likibu.LikibuAccess.ConvertDateString(searchVM.Search.CheckOut);
                        
            guests = (searchVM.Search.BedRooms * 2).ToString();             
            culture = searchVM.Language;
            currency = searchVM.Currency;
            Body = "&where=" + where + "&checkin=" + checkin + "&checkout=" + checkout + "&guests=" + guests + "&culture=" + culture + "&currency=" + currency;
        }
        
    }
    public class StartSearchSessionReturnObject
    {
        public string ID { get; set; }
        public string StatusPollAdress { get; set; }
        public string ResultPollAdress { get; set; }
        public StartSearchSessionReturnObject(string id, string statusAd, string resAd)
        {
            ID = id;
            StatusPollAdress = statusAd;
            ResultPollAdress = resAd;
        }
    }
    public class AccomodationDetailsParam
    {

        private string culture { get; set; }
        private string currency { get; set; }
        private string checkin { get; set; }
        private string checkout { get; set; }
        public string Body { get; }
        public AccomodationDetailsParam(string Culture, string Currency, string CheckIn, string CheckOut)
        {
            culture = Culture;
            currency = Currency;
            checkin = Controllers.Likibu.LikibuAccess.ConvertDateString(CheckIn);
            checkout = Controllers.Likibu.LikibuAccess.ConvertDateString(CheckOut);
            Body = "&checkin=" + checkin + "&checkout=" + checkout + "&culture=" + culture + "&currency=" + currency;
        }
        
    }

    public class PollSearchSessionResponse
    {
        public string SearchID;
        public string Where;
        public string Country;
        public string Bbox;
        public string Status;
        public string Partners;

        public PollSearchSessionResponse(string searchid, string where, string country, string bbox, string status, string partners)
        {
            SearchID = searchid;
            Where = where;
            Country = country;
            Bbox = bbox;
            Status = status;
            Partners = partners;
        }
    }

    public class SearchResultsRequestBody
    {
        public string Body { get; }
        public SearchResultsRequestBody(string per_page, string page, string sort, string privacy, string type, string amenities, string partnerid, string guests, string pricemin, string pricemax)
        {
            Body = "&";
            if (per_page != "") { Body += "per_page=" + per_page; }
            if (page != "") { Body += "&page=" + page; }
            if (sort != "") { Body += "&sort=" + sort; }
            if (privacy != "") { Body += "&privacy_type=" + privacy; }
            if (amenities != "") { Body += "&amenities=" + amenities; }
            if (partnerid != "") { Body += "&partner_id=" + partnerid; }
            if (guests != "") { Body += "&guests=" + ((int.Parse(guests)*2).ToString()); }
            if (pricemin != "") { Body += "&price_min=" + pricemin; }
            if (pricemax != "") { Body += "&price_max=" + pricemax; }
        }
    }
    public class SearchRequestResults
    {
        [Key]
        public string SearchID { get; set; }
        public string Status { get; set; }
        public string Bbox { get; set; }
        public string Where { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string CountryCode { get; set; }
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public List<Offer> Offers { get; set; }
    }
    public class AccomodationDetails
    {
        public int ID { get; set; }
        public Offer offer { get; set; }
        [Display(Name = "Availability: ")]
        public string availability { get; set; }
        [Display(Name = "Total Price: ")]
        public float total_price { get; set; }
    }
}
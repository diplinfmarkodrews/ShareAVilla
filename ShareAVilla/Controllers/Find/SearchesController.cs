using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareAVilla.Models;
using Microsoft.AspNet.Identity;

namespace ShareAVilla.Controllers
{
     
    public class SearchesController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet] ///Todo load old Search
        public async Task<ActionResult> Search()
        {
            
            try
            {
                if (Request.IsAuthenticated)
                {
                  
                    TravelerProfile travelerProfile = await ProfileManager.LoadUserAndTravelerProfile(User);
                    if (!Check.IsNull(travelerProfile, "Search"))
                    {

                        travelerProfile.SearchSession = await db.LikibuSearchSession.FindAsync(travelerProfile.TravelerSearchID);
                        if (!Check.IsNull(travelerProfile.SearchSession, "Search"))
                        {

                            TimeSpan lastSearch = DateTime.Now - travelerProfile.SearchSession.TimeStamp;
                            TimeSpan oneDay = TimeSpan.FromDays(1);
                            if (lastSearch < oneDay)
                            {
                                Models.Find.SearchVM searchVM = new Models.Find.SearchVM();
                                searchVM.Search = new Models.Find.Search();
                                searchVM.Search.CheckIn = travelerProfile.SearchSession.CheckIn;
                                searchVM.Search.CheckOut = travelerProfile.SearchSession.CheckOut;
                                searchVM.Search.Location = travelerProfile.SearchSession.Location;
                                searchVM.Search.BedRooms = travelerProfile.SearchSession.BedRooms;
                                searchVM.Search.FilterProps = travelerProfile.SearchSession.Filter;
                                return await Search(searchVM);
                            }
                        }
                    }
                    

                }

                return View();
            }
            catch (Exception e) {
                NotificationManager.AddException(e);
                return RedirectToAction("Index", "Home");
            }

        }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(Models.Find.SearchVM searchVM)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    if (searchVM.Search.nav == null)
                    {
                        if (Request.IsAuthenticated)
                        {

                            TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                            tp.Attributes = await db.TravelerProfileAttributes.FindAsync(tp.TravelerAttributesID);
                            searchVM.Currency = tp.Attributes.Currency;
                            searchVM.Language = tp.Attributes.Language;

                        }
                        else
                        {


                            Models.Find.Search search = searchVM.Search;
                            searchVM = new Models.Find.SearchVM();
                            searchVM.Search = search;
                            searchVM.Currency = "usd";
                            searchVM.Language = "en";
                        }

                        await StartLikibuSearch(searchVM); /// if ret == null Show internal Server Error, try again later
                        await WaitForResults(searchVM);
                        await GetSearchResults(searchVM);

                        await GetRequestSearchResults(searchVM); 
                    }
                    else
                    {
                        int page = int.Parse(searchVM.Search.FilterProps.Page);
                        switch (searchVM.Search.nav)
                        {
                            case "p":
                                {
                                    if (page > 1) { page--; }
                                    break;
                                }
                            case "n":
                                {
                                    if (page < searchVM.LikibuResults.TotalPages)
                                        page++;
                                    break;
                                }
                            default:
                                break;

                        }
                        searchVM.Search.FilterProps.Page = page.ToString();

                     

                        await GetRequestSearchResults(searchVM);
                        await GetSearchResults(searchVM);
                        
                    }
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                    
                }
                return View(searchVM);
            }//ModelState not valid
           
            return View(searchVM);
        }
        
        private async Task<Models.Find.RequestResults> GetRequestSearchResults(Models.Find.SearchVM searchVM)
        {
            string destination = searchVM.Search.Location;
            Models.Accommodation.Likibu.Destination dest = (await GetGeolocationString(destination, searchVM.Language, "1")).FirstOrDefault();

            string destID = dest.ID;

            using (var db = new ApplicationDbContext())
            {
                IQueryable<Models.Request.Request> retQ = db.Requests.Where(p => p.Accommodation.AccomProfile.DestinationIDs.Contains(destID)).Include(p=>p.Accommodation).Include(p=>p.Accommodation.AccomProfile); /// Destination Ids have to be updated
                //retQ = FilterCheckTimeRequests(searchVM, retQ, 3.0f);
                retQ = retQ.Where(p => p.objState == BaseDataObject.ObjectState.Valid);
                int totalres = retQ.Count();
                int page = int.Parse(searchVM.Search.FilterProps.Page);
                int perpage = int.Parse(searchVM.Search.FilterProps.PerPage);
                IEnumerable<Models.Request.Request> retList = await retQ.ToListAsync();

                if (page > 1)
                {
                    retList = retList.Skip(perpage * (page - 1));
                }
                retList = retList.Take(perpage);


                List<Models.Request.RequestVMListing> retListFiltered = new List<Models.Request.RequestVMListing>();
                
                foreach (var rq in retList) //Filter invalid requests
                {
                    if (rq.IsValid() )
                    {
                        Models.VM.ProfilePreviewVM host = await ProfilesController.ProfilePreviewVM(rq.RequestOwner_ID);
                        retListFiltered.Add(new Models.Request.RequestVMListing(rq, "", host)); //stats
                       
                    }

                }

                searchVM.RequestResults = new Models.Find.RequestResults(retListFiltered, page, perpage, totalres); //int.Parse(searchVM.Search.FilterProps.Page), int.Parse(searchVM.Search.FilterProps.PerPage)                              
                return searchVM.RequestResults;
            }
        }
        
        //Poll Status until server is ready to get Results
        private async Task<bool> WaitForResults(Models.Find.SearchVM searchVM)
        {    
            try
            {
                while (true)
                {
                    await Task.Delay(1000);
                    await PollSearchStatus(searchVM);
                    if (searchVM.Search.SessionStatus)
                    {                        
                        return true;
                    }
                    await Task.Delay(100);
                }                
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return false;
            }
        }

        /// <summary>
        /// Save Results to SearchVM 
        /// Save SearchSession to db if user is registered -->needed to Create Request
        /// </summary>
        /// <param name="searchVM"></param>
        /// <returns></returns>
        private async Task<bool> GetSearchResults(Models.Find.SearchVM searchVM)
        {
            try
            {
                searchVM.LikibuResults = new Models.Accommodation.Likibu.SearchRequestResults();
                searchVM.LikibuResults = await Likibu.LikibuAccess.RequestSearchResults(searchVM);
                if (Request.IsAuthenticated)
                {
                    await UpdateSearchSession(searchVM);
                }
                return true;
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return false;
            }
        }
        private async Task<bool> UpdateSearchSession(Models.Find.SearchVM searchVM)
        {
            try
            {
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                using (var db = new ApplicationDbContext())
                {
                   
                    TravelerProfile travelerDB = db.TravelerProfile.Find(tp.ID);
                    var trackedSearch = db.LikibuSearchSession.Find(travelerDB.TravelerSearchID);
                    
                    tp.SearchSession.UpdateSearchSession(searchVM.Search);
                    db.Entry(trackedSearch).CurrentValues.SetValues(tp.SearchSession);
                    db.Entry(trackedSearch.Filter).CurrentValues.SetValues(tp.SearchSession.Filter);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return false;
            }
        }
        //SearchStatus to SearchVM
        private async Task<Models.Accommodation.Likibu.PollSearchSessionResponse> PollSearchStatus(Models.Find.SearchVM searchVM)
        {
            Models.Accommodation.Likibu.PollSearchSessionResponse response = await Likibu.LikibuAccess.PollSearchStatus(searchVM.Search.LikibuSessionID);
            searchVM.Search.SessionStatus = bool.Parse(response.Status); 
            
            return response;
        }
        
        private async Task<bool> StartLikibuSearch(Models.Find.SearchVM searchVM)
        {
            try
            {
               
                InitSearchVM4Search(searchVM);               
                string searchID = await Likibu.LikibuAccess.StartSearchSession(searchVM);
                if (searchID == null) throw new Exception("No SearchID returned by Likibu request");
                PopulateSearchSession(searchVM, searchID);                
                return true;
               
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                if (e.GetType() == typeof(WebException))
                {
                    NotificationManager.AddNotification(NotificationType.Warning, "Internal Server Error! Please be patient, we are working on it...");
                }
            }

            return false;

        }

        private void InitSearchVM4Search(Models.Find.SearchVM searchVM)
        {
            searchVM.Search.FilterProps = new Models.Find.FilterProperties();
            searchVM.Search.FilterProps.PerPage = "10";
            searchVM.Search.FilterProps.Page = "1";
            searchVM.Search.FilterProps.Sort = "reco";// Models.Find.Likibu.Sort.Reco;
            searchVM.Search.FilterProps.AcomType = "";
            searchVM.Search.FilterProps.PartnerID = "";
            searchVM.Search.FilterProps.PrivacyType = "";
            searchVM.Search.FilterProps.Amenities = "";
            searchVM.Search.FilterProps.PriceMin = "";
            searchVM.Search.FilterProps.PriceMax = "";
            
        }
        private void PopulateSearchSession(Models.Find.SearchVM searchVM, string searchID)
        {
            
            searchVM.Search.LikibuSessionID = searchID;            
            searchVM.Search.TimeStamp = DateTime.Now;
            
        }
        [HttpGet]        
        public async Task<JsonResult> ShowGeoLocationString(string LocationString, string lang)
        {
            try
            {
                var dest = await GetGeolocationString(LocationString, lang, "10");
                JsonResult res = new JsonResult();
                res.Data = dest;
                res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return res;
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
            }
            return null;
        }
        /// <summary>
        /// use DestinationObject for Locationing
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static async Task<List<Models.Accommodation.Likibu.Destination>> GetGeolocationString(string Location, string lang, string numSug)
        {
            
            List<Models.Accommodation.Likibu.Destination> dest;
            dest = await Likibu.LikibuAccess.DestinationRequest(Location, lang, numSug);
            return dest;
        }
        
        [HttpPost]
        public async Task<JsonResult> FetchSearchResults(Models.Find.SearchVM searchVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    JsonResult res = new JsonResult();
                    
                    searchVM.LikibuResults = await Likibu.LikibuAccess.RequestSearchResults(searchVM);
                    searchVM.RequestResults = await GetRequestSearchResults(searchVM);
                    res.Data = searchVM;
                    return res;

                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                }
            }
            return null;
        }
        public async Task<ActionResult> Details(string lang, string cur, string chIn, string chOut, string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Models.Accommodation.Likibu.AccomodationDetails details = await GetAcomodationDetails(lang, cur, chIn, chOut, id);
                    return View(details.offer);
                }
                return View();

            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
            }
            return PartialView();
        }
        private async Task<Models.Accommodation.Likibu.AccomodationDetails> GetAcomodationDetails(Models.Find.SearchVM searchVM, string roomID)
        {
            Models.Accommodation.Likibu.AccomodationDetailsParam reqParam = new Models.Accommodation.Likibu.AccomodationDetailsParam(searchVM.Language, searchVM.Currency, searchVM.Search.CheckIn, searchVM.Search.CheckOut);
            Models.Accommodation.Likibu.AccomodationDetails result = await Likibu.LikibuAccess.RequestAccomodationDetails(reqParam, roomID);
            return result;
           
        }
        private async Task<Models.Accommodation.Likibu.AccomodationDetails> GetAcomodationDetails(string lang, string cur, string chIn, string chOut , string roomID)
        {
            Models.Accommodation.Likibu.AccomodationDetailsParam reqParam = new Models.Accommodation.Likibu.AccomodationDetailsParam(lang, cur, chIn, chOut);
            Models.Accommodation.Likibu.AccomodationDetails result = await Likibu.LikibuAccess.RequestAccomodationDetails(reqParam, roomID);
            return result;

        }
        
       
        private IQueryable<Models.Request.Request> FilterCheckTimeRequests(Models.Find.SearchVM searchVM, IQueryable<Models.Request.Request> retQ, float checkOffset)
        {
            
            DateTime chckIn = DateTime.Parse(searchVM.Search.CheckIn);
            chckIn.AddDays(checkOffset);
            DateTime chckOut = DateTime.Parse(searchVM.Search.CheckOut);
            chckOut.AddDays(-checkOffset);
           
            if (searchVM.Search.CheckIn != null && searchVM.Search.CheckOut != null)
            {
                IQueryable<Models.Request.Request> excludeBefore = retQ.Where(p => p.CheckIn > chckOut);
                IQueryable<Models.Request.Request> excludeAfter = retQ.Where(p => p.CheckOut < chckIn);

                retQ = retQ.Except(excludeBefore);
                retQ = retQ.Except(excludeAfter);
            }
            return retQ;
        }
        /// <summary>
        /// filters number of bedrooms according to search in searchVM.Search(SearchMask) and AccommodationProfile
        /// </summary>
        /// <param name="searchVM"></param>
        /// <param name="retQ"></param>
        /// <returns>Iqueryable with filtered results</returns>
        
        private IQueryable<Models.Request.Request> FilterBedRooms(Models.Find.SearchVM searchVM, IQueryable<Models.Request.Request> retQ)
        {
            retQ = retQ.Where(p => p.Accommodation.AccomProfile.BedRooms >= searchVM.Search.BedRooms);
            return retQ;
        }
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}

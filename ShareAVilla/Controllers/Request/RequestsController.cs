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


namespace ShareAVilla.Controllers.Request
{
    
    public class RequestsController : Controller
    {


        /// <summary>
        /// Get
        /// Display all Requests of a User
        /// </summary>
        /// <returns></returns>


        [Authorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                tp = await ProfilesController.LoadProfile(tp.ID);
                using (var db = new ApplicationDbContext())
                {
                    IQueryable<Models.Request.Request> IQuery = db.Requests.Where(p => p.RequestOwner_ID == tp.ID).Include(p => p.Accommodation).Include(p => p.Accommodation.AccomProfile);
                    List<Models.Request.Request> requestList = await IQuery.ToListAsync();
                    List<Models.Request.RequestVMListing> resultList = new List<Models.Request.RequestVMListing>();

                    foreach (var rq in requestList)
                    {
                        if (!rq.IsDeleted())
                        {
                            Models.VM.ProfilePreviewVM host = await ProfilesController.ProfilePreviewVM(rq.RequestOwner_ID);
                            string rrqstats = await RoomRequest.RoomRequestsController.RoomRequestStats(rq);
                            resultList.Add(new Models.Request.RequestVMListing(rq, rrqstats, host));
                        }
                    }
                    return View(resultList);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return View();
            }
        }


        /// <summary>
        /// Display Details of a certain Request to User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Requests/Details/5

        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    id = id.Value;
                }
                else { return View(); }

                using (var db = new ApplicationDbContext())
                {
                    Models.Request.Request request = await db.Requests.FindAsync(id);

                    if (Check.IsNull(request, "RequestDetails"))
                    {

                        return RedirectToAction("Search", "Searches");
                    }
                    if (request.IsDeleted("RequestDetails"))
                    {
                        return RedirectToAction("Search", "Searches");
                    }
                    ///Loads Accommodation with Profile

                    request.Accommodation = await AccommodationsController.LoadAccommodation(request.Accommodation_ID);

                    Models.VM.ProfilePreviewVM rqOwnerVM = await ProfilesController.ProfilePreviewVM(request.RequestOwner_ID);
                    
                    Models.Request.RequestVMUser requestVM = new Models.Request.RequestVMUser(request, rqOwnerVM);
                    return View(requestVM);
                }
            }
            catch (Exception e) {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return View();

            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Reservations(int? id) //RequestID
        {
            try
            {
                if (!id.HasValue)
                {
                    return RedirectToAction("Index", "Home");
                }
                id = id.Value;
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                if (Check.IsNull(tp, "Index RoomRequests"))
                {

                    return RedirectToAction("Index", "Home");
                }
                using (var db = new ApplicationDbContext())
                {
                    Models.Request.Request rq = await db.Requests.FindAsync(id);
                    rq.Accommodation = await AccommodationsController.LoadAccommodation(rq.Accommodation_ID); 
                    


                    IQueryable<Models.RoomRequest.RoomRequest> IQuery = db.RoomRequests.Where(p => p.Request_ID == id).Include(p => p.RoomRequestResponse);
                    IQueryable<Models.RoomRequest.RoomRequest> Reservations = IQuery.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Reserved);
                    IQueryable<Models.RoomRequest.RoomRequest> Approvals = IQuery.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Approved);
                    List<Models.RoomRequest.RoomRequest> ReservationsList = await Reservations.ToListAsync();
                    List<Models.RoomRequest.RoomRequestVMOwner> resListVM = new List<Models.RoomRequest.RoomRequestVMOwner>();
                    List<Models.RoomRequest.RoomRequest> ApprovalsList = await Approvals.ToListAsync();
                    List<Models.RoomRequest.RoomRequestVMOwner> apprListVM = new List<Models.RoomRequest.RoomRequestVMOwner>();
                    foreach (var res in ReservationsList)
                    {
                        
                        Models.VM.ProfilePreviewVM tpVMPrev = await ProfilesController.ProfilePreviewVM(res.ApplyingTraveler_ID);
                        Models.RoomRequest.RoomRequestVMOwner rRqVM = new Models.RoomRequest.RoomRequestVMOwner(res, tpVMPrev);
                        resListVM.Add(rRqVM);
                    }
                    foreach (var appr in ApprovalsList)
                    {                       
                        Models.VM.ProfilePreviewVM tpVMPrev = await ProfilesController.ProfilePreviewVM(appr.ApplyingTraveler_ID);
                        Models.RoomRequest.RoomRequestVMOwner rRqVM = new Models.RoomRequest.RoomRequestVMOwner(appr, tpVMPrev);
                        apprListVM.Add(rRqVM);
                    }
                    Models.RoomRequest.ReservationsVM reservationsVM = new Models.RoomRequest.ReservationsVM(rq.Accommodation.BedRooms, resListVM, apprListVM, rq);


                    return View(reservationsVM);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return View();
            }
        }


        //protected void Page_Load(object sender, EventArgs e)
        //{
        //     if (!IsPostBack)
        //{
        //  LoadResources();
        //  InitCalendar();
        //  }
        //LoadEvents();
        //}


        //Details for RequestOwner, shows Pricedetails and RoomRequests
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> DetailsHost(int? id) //RequestID
        {
            try
            {
                if (!id.HasValue)
                {
                    return RedirectToAction("Index", "Requests");
                }
                id = id.Value;
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                if (Check.IsNull(tp, "DetailsOwner"))
                {
                    return RedirectToAction("Create", "Profiles");
                }
                using (var db = new ApplicationDbContext())
                {
                    Models.Request.Request request = await db.Requests.FindAsync(id);

                    if (Check.IsNull(request, "DetailsOwner"))
                    {
                        return RedirectToAction("Index", "Requests");
                    }
                    if (request.IsDeleted("DetailsOwner"))
                    {
                        return RedirectToAction("Index", "Requests");
                    }

                    if (request.RequestOwner_ID != tp.ID)
                    {
                        NotificationManager.AddException(new Exception("Access violation, DetailsOwner, RqCtrl"));
                        return RedirectToAction("Index", "Requests");
                    }

                    string rrqstats = await RoomRequest.RoomRequestsController.RoomRequestStats(request);

                    request.Accommodation = await db.Accommodations.FindAsync(request.Accommodation_ID);
                    request.Accommodation.AccomProfile = await db.AccommProfile.FindAsync(request.Accommodation.AccomProfileID);
                    Models.Request.RequestVMOwner requestVM = new Models.Request.RequestVMOwner(request, rrqstats);
                    return View(requestVM);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return RedirectToAction("Index", "Requests");
            }
        }


        //Todo later feature
        [Authorize]
        [HttpGet]
        public ActionResult CreateFree()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateFree(Models.Request.RequestVMFree requestVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                    tp = await ProfilesController.LoadProfile(tp.ID);
                    Models.Request.Request rq = new Models.Request.Request(tp.ID, requestVM); //AccommodationModel is generated here

                    rq.Accommodation = new Models.Accommodation.Accommodation(0, requestVM.BedRooms);
                    if (!rq.Accommodation.Verified)
                    {
                        throw new Exception("Accommodation abstraction failed");

                    }
                    // get locationIDs
                    List<Models.Accommodation.Likibu.Destination> dest = await Likibu.LikibuAccess.DestinationRequest(requestVM.Location, tp.Attributes.Language, 1.ToString());
                    //Models.Accommodation.Likibu.DestinationDetail destDetail = await Likibu.LikibuAccess.DestinationDetailRequest();// Todo add LatLng
                    

                    rq.Accommodation.AccomProfile = new Models.Accommodation.AccommodationProfile(dest.FirstOrDefault(), requestVM.BedRooms, requestVM.Link);
                    if (!requestVM.Lat.HasValue || !requestVM.Lng.HasValue)
                    {
                        float[] lnglat = new float[2];
                        lnglat = GoogleWrapper.LongLat2DoubleAr(await GoogleWrapper.GetGeolocation(requestVM.Location));

                        rq.Accommodation.AccomProfile.Lng = lnglat[0];
                        rq.Accommodation.AccomProfile.Lat = lnglat[1];
                    }
                    else
                    {
                        rq.Accommodation.AccomProfile.Lng = requestVM.Lng.Value;
                        rq.Accommodation.AccomProfile.Lat = requestVM.Lat.Value;
                    }
                    using (var db = new ApplicationDbContext())
                    {
                        db.Requests.Add(rq);
                        await db.SaveChangesAsync();
                    }
                    NotificationManager.AddNotification(NotificationType.Success, "Your request has been saved!");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                    
                }
            }
            return View(requestVM);
        }

        [Authorize]
        // GET: Requests/Create //id is roomid likibu
        public async Task<ActionResult> Create(string id, string thumb, string Location)
        {
            try
            {
                //load Offer from Likibu SearchSession
                if (User != null)
                {

                    TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                    TravelerProfileSearchSession searchsession = tp.SearchSession; ///searchsession comes here fom view? any has to be saved in loaded TravelerProfile
                    tp = await ProfilesController.LoadProfile(tp.ID);
                    tp.SearchSession = searchsession;
                    Models.Accommodation.Likibu.AccomodationDetailsParam para = new Models.Accommodation.Likibu.AccomodationDetailsParam(tp.Attributes.Language, tp.Attributes.Currency, tp.SearchSession.CheckIn, tp.SearchSession.CheckOut);
                    Models.Accommodation.Likibu.AccomodationDetails details = await Likibu.LikibuAccess.RequestAccomodationDetails(para, id);
                    Models.Request.RequestVMOwner requestVM = new Models.Request.RequestVMOwner(details.offer.Destination, details.offer.BedRooms, details.offer.PriceNight, details.offer.PriceWeek, details.offer.PriceMonth, details.offer.Lat, details.offer.Lng, tp.SearchSession.CheckIn, tp.SearchSession.CheckOut, id, tp.Attributes.Currency, thumb);
                    
                    requestVM.Location = Location;
                    requestVM.Thumbs = Models.Accommodation.Likibu.Offer.StringToArray(thumb).ToList();


                    return View(requestVM);
                }
                return View();
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return View();
            }
            
        }

        // POST: Requests/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude = "ID")] Models.Request.RequestVMOwner requestVM)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                    Models.Accommodation.Likibu.AccomodationDetailsParam para = new Models.Accommodation.Likibu.AccomodationDetailsParam(tp.Attributes.Language, tp.Attributes.Currency, requestVM.CheckIn.ToString(), requestVM.CheckOut.ToString());
                    Models.Accommodation.Likibu.AccomodationDetails details = await Likibu.LikibuAccess.RequestAccomodationDetails(para, requestVM.AccomID);
                    ///
                    using (var db = new ApplicationDbContext())
                    {

                        requestVM.BedRooms = details.offer.BedRooms;
                        requestVM.Lat = details.offer.Lat;
                        requestVM.Lng = details.offer.Lng;
                        requestVM.PricePNight = details.offer.PriceNight.ToString();
                        requestVM.PricePWeek = details.offer.PriceWeek.ToString();
                        requestVM.PricepMonth = details.offer.PriceMonth.ToString();

                        details.offer.Destination = requestVM.Location;
                        details.offer.Thumbnails = Models.Accommodation.Likibu.Offer.StringToArray(requestVM.ThumbsTemp).ToList();



                        Models.Request.Request rq = new Models.Request.Request(tp.ID, requestVM);

                        rq.Accommodation = new Models.Accommodation.Accommodation(details.offer.MaxGuests, details.offer.BedRooms); //AccommodationModel is generated here
                        if (!rq.Accommodation.Verified)
                        {
                            throw new Exception("Accommodation abstraction failed");

                        }
                        string currency = ProfilesController.Currency(tp.Attributes.Currency);
                        rq.Accommodation.AccomProfile = new Models.Accommodation.AccommodationProfile(details.offer, currency);

                        db.Requests.Add(rq);
                        await db.SaveChangesAsync();

                        NotificationManager.AddNotification(NotificationType.Success, "Your Request has been saved!");

                        return RedirectToAction("Create", "RoomRequests", new { id = rq.ID });
                    }
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                    return RedirectToAction("Search", "Searches");
                }
            }
            return View(requestVM);

        }



        /// <summary>
        /// Finds rooms appropriate for roomrequest
        /// </summary>
        /// <param name="roomRequest"></param>
        /// <returns></returns>
        //public static async Task<List<Models.Accommodation.BedRoom>> GetSuitingRooms4RoomRequest(Models.RoomRequest.RoomRequest roomRequest)
        //{
        //    List<Models.Accommodation.BedRoom> roomsWCapacity = await GetFreeRooms(roomRequest.CheckInUser, roomRequest.CheckOutUser, roomRequest.Request_ID);
        //    List<Models.Accommodation.BedRoom> Rooms = new List<Models.Accommodation.BedRoom>();
        //    foreach (var room in roomsWCapacity)
        //    {
        //        if (!roomRequest.ShareRoom)
        //        {
        //            if (room.Vacant)
        //            {
        //                if (room.SingleBeds >= roomRequest.SingleBed && room.DoubleBeds >= roomRequest.DoubleBed)
        //                {
        //                    Rooms.Add(room);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (room.SingleBeds >= roomRequest.SingleBed && room.DoubleBeds >= roomRequest.DoubleBed)
        //            {
        //                Rooms.Add(room);
        //            }
        //        }





        //    }

        //    return null;


        //}


        //public static async Task<List<Models.Accommodation.BedRoom>> GetFreeRooms(DateTime CheckIn, DateTime CheckOut, int rqID)
        //{
            //using (var db = new ApplicationDbContext())
            //{
            //    Models.Request.Request request = db.Requests.Find(rqID);
            //   // Models.Accommodation.Accommodation accom = db.Accommodations.Where(p => p.ID == request.Accommodation_ID).Include(p => p.BedRooms).Include(p => p.Capacity).FirstOrDefault();
            //   // int numbedRooms = accom.BedRooms.Count;
            //    IQueryable<Models.RoomRequest.RoomRequest> RoomRequests = db.RoomRequests.Where(p => p.Request_ID == rqID).Include(p => p.RoomRequestResponse); //Find all roomRequests and its response
            //    RoomRequests = RoomRequests.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Reserved); //Filter reserved RoomRequests
            //    if (RoomRequests == null || RoomRequests.ToList().Count == 0) //no reservation was made, all Bedrooms are vacant
            //    {
            //     //   return accom.BedRooms;
            //    }
            //    IQueryable<Models.RoomRequest.RoomRequest> RoomRequestsOutSmaller = RoomRequests.Where(p => p.CheckInUser < CheckIn && p.CheckOutUser < CheckOut); //exclude outlayers before
            //    IQueryable<Models.RoomRequest.RoomRequest> RoomRequestsOutBigger = RoomRequests.Where(p => p.CheckInUser > CheckIn && p.CheckOutUser > CheckOut); //exclude outlayer after
            //    RoomRequests = RoomRequests.Except(RoomRequestsOutSmaller); //Except outlayers
            //    RoomRequests = RoomRequests.Except(RoomRequestsOutBigger);
            //    //List<Models.RoomRequest.RoomRequest> filteredRoomRequestsReserved = await RoomRequests.Where(p => p.RoomRequestResponse.IsReserved()).ToListAsync();

            //    List<Models.RoomRequest.RoomRequest> filteredRoomRequestsReserved = await RoomRequests.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Reserved).ToListAsync(); //catch empty List

            //    if (filteredRoomRequestsReserved == null || filteredRoomRequestsReserved.Count == 0) //no reservation was made, all Bedrooms are vacant
            //    {
            //       // return accom.BedRooms;
            //    }
            //    List<Models.Accommodation.BedRoom>[] BookedBedrooms = new List<Models.Accommodation.BedRoom>[numbedRooms];
            //    foreach (var rRq in filteredRoomRequestsReserved) //FindBedrooms and its occupation for the given time interval
            //    {
            //        var room = db.BedRooms.Find(rRq.Room_ID);

            //        BookedBedrooms[room.RoomNumber - 1].Add(room);
            //    }

            //    List<Models.Accommodation.BedRoom> FreeBedRooms = new List<Models.Accommodation.BedRoom>();
            //    for (int i = 0; i < numbedRooms; i++) //check if rooms are fully loaded
            //    {
            //        if (BookedBedrooms[i].Count == 0)
            //        {
            //            Models.Accommodation.BedRoom freeBedRoom = accom.BedRooms.Where(p => p.RoomNumber == i).FirstOrDefault(); //just add the roomspecification
            //            freeBedRoom.Vacant = true;
            //            FreeBedRooms.Add(freeBedRoom);
            //        }
            //        else //get detailed Booking status ->check the roomrequests for each room in that time //
            //        {
            //            List<Models.RoomRequest.RoomRequest> rRqRoom = filteredRoomRequestsReserved.Where(p => p.Room_ID == BookedBedrooms[i].FirstOrDefault().ID).ToList();
            //            int doublebedsOccupied = 0;
            //            int singlebedsOccupied = 0;
            //            bool shared = true;
            //            foreach (var rRq in rRqRoom)
            //            {
            //                doublebedsOccupied += rRq.DoubleBed;
            //                singlebedsOccupied += rRq.SingleBed;
            //                if (rRq.ShareRoom != shared) { shared = false; }
            //            }

            //            if (shared && doublebedsOccupied + singlebedsOccupied < BookedBedrooms[i].FirstOrDefault().MaxGuests)
            //            {
            //                Models.Accommodation.BedRoom SharedBedRoom = BookedBedrooms[i].FirstOrDefault();
            //                SharedBedRoom.DoubleBeds -= doublebedsOccupied;
            //                SharedBedRoom.SingleBeds -= singlebedsOccupied;
            //                SharedBedRoom.SharePossible = true;     //these rooms are shared in this moment    
            //                FreeBedRooms.Add(SharedBedRoom);
            //            }

            //        }

            //    }

            //    return FreeBedRooms;
            //}
       // }
       
        //GET: Requests/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
           
            using (var db = new ApplicationDbContext())
            {
                Models.Request.Request request = await db.Requests.FindAsync(id);
                request.Accommodation = await AccommodationsController.LoadAccommodation(request.Accommodation_ID);
                
                if (Check.IsNull(request, "Edit Request"))
                {
                    NotificationManager.AddNotification(NotificationType.Warning, "Request does not exist!");
                    return RedirectToAction("Index", "Home");
                    
                }
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                if (request.RequestOwner_ID != tp.ID)
                {
                    NotificationManager.AddException(new Exception("User is not the Requestowner, Access Violation"));
                    NotificationManager.AddNotification(NotificationType.Warning, "Access Violation");
                    return RedirectToAction("Index", "Home");
                }
                
               
                
                Models.Request.RequestVMOwner rqVM = new Models.Request.RequestVMOwner(request);
                
                return View(rqVM);
            }
        }

        // POST: Requests/Edit/5
        // Properties have to be done
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.Request.RequestVMOwner requestVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                    using (var db = new ApplicationDbContext())
                    {
                        
                        var trackedRequest = await db.Requests.FindAsync(requestVM.ID);
                        trackedRequest.Accommodation = await db.Accommodations.FindAsync(trackedRequest.Accommodation_ID);
                        trackedRequest.Accommodation.AccomProfile = await db.AccommProfile.FindAsync(trackedRequest.Accommodation.AccomProfileID);
                        Models.Request.Request update = trackedRequest;
                        update.SetValues(requestVM);

                        db.Entry(trackedRequest).CurrentValues.SetValues(update);
                        await db.SaveChangesAsync();
                        NotificationManager.AddNotification(NotificationType.Success, "Your request has been updated succesfully");
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                    return View(requestVM);
                }
            }
            return View(requestVM);
        }

        /// <summary>
        /// Return all RoomRequests for a certain request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]  ///Todo implement Reservation Validation and Edit View appropriate    Preselection  of Profiles 
        public async Task<ActionResult> SelectRoom(int id)  // RoomRequest id
        {
            try
            {
                
                using (var db = new ApplicationDbContext())
                {
                    Models.RoomRequest.RoomRequest roomRequest = db.RoomRequests.Find(id);
                    roomRequest.RoomRequestResponse = await db.RoomRequestResponse.FindAsync(roomRequest.RoomRequestResponse_ID);
                    
                    Models.VM.ProfilePreviewVM applicantPrevVM = await ProfilesController.ProfilePreviewVM(roomRequest.ApplyingTraveler_ID);
                    List<Models.RoomRequest.RoomRequest> reservations = await db.RoomRequests.Where(p => p.Request_ID == roomRequest.Request_ID).Include(p => p.RoomRequestResponse).Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Reserved).ToListAsync();
                    Models.Request.Request rq = db.Requests.Where(p => p.ID == roomRequest.Request_ID).Include(p => p.Accommodation).Include(p => p.Accommodation.BedRooms).FirstOrDefault();
                    Models.RoomRequest.RoomRequestVMOwner applicantVM = new Models.RoomRequest.RoomRequestVMOwner(roomRequest, applicantPrevVM);

                    Models.Request.SelectRoomVM sRVM = new Models.Request.SelectRoomVM(applicantVM, reservations, rq.Accommodation.BedRooms);
                    return View(sRVM);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return View();
            }
        }

        [Authorize]
        [HttpGet]  
        public async Task<ActionResult> ReserveRoom(int id, int idrrq)  //RoomID
        {
            try
            {
                
                using (var db = new ApplicationDbContext())
                {
                    Models.RoomRequest.RoomRequest rRq = await db.RoomRequests.FindAsync(idrrq);
                    rRq.RoomRequestResponse = await db.RoomRequestResponse.FindAsync(rRq.RoomRequestResponse_ID);
                    Models.RoomRequest.RoomRequest rRqUpdate = rRq;
                    rRqUpdate.Reserve(id);
                    db.Entry(rRq).CurrentValues.SetValues(rRqUpdate);
                    db.Entry(rRq.RoomRequestResponse).CurrentValues.SetValues(rRqUpdate.RoomRequestResponse);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Reservations", "Requests", new { id = rRq.Request_ID });
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return View();
            }
        }
                // GET: Requests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Check.IsNull(id))
            {
                NotificationManager.AddException(new Exception("Request Delete, ID was null"));
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong");
                return View();
            }
            TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
            using (var db = new ApplicationDbContext())
            {
                Models.Request.Request request = await db.Requests.FindAsync(id);
                if (Check.IsNull(request,"DeleteRequest"))
                {
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong");
                    return View();
                }
                if(request.RequestOwner_ID != tp.ID)
                {
                    NotificationManager.AddException(new Exception("Access Violation, Requesteditor is not requestowner"));
                }
                request.Accommodation = await AccommodationsController.LoadAccommodation(request.Accommodation_ID);
                Models.Request.RequestVMOwner rqVM = new Models.Request.RequestVMOwner(request);
                return View(rqVM);
            }
        }
       
        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
            using (var db = new ApplicationDbContext())
            {
                Models.Request.Request requestDB = await db.Requests.FindAsync(id);
                if (requestDB.RequestOwner_ID != tp.ID)
                {
                    NotificationManager.AddException(new Exception("Access Violation, Requesteditor is not requestowner"));
                }
                IQueryable<Models.RoomRequest.RoomRequest> RoomRequestsToBeCanceled = db.RoomRequests.Where(p => p.Request_ID == id);
                List<Models.RoomRequest.RoomRequest> rRqL = await RoomRequestsToBeCanceled.ToListAsync();
                foreach (var rRq in rRqL)
                {
                    rRq.Invalid();          //Set RoomRq toinvalid, user will see request was Deleted
                    db.Entry(rRq).State = EntityState.Modified;
                }


                requestDB.Delete();
                db.Entry(requestDB).State = EntityState.Modified;       
                await db.SaveChangesAsync();
                NotificationManager.AddNotification(NotificationType.Success, "Your request has been deleted");
            }
            
            return RedirectToAction("Index");
        }

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

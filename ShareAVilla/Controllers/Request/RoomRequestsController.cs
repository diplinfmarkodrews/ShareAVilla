using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareAVilla.Models;
using ShareAVilla.Models.Request;
using System.Threading.Tasks;
namespace ShareAVilla.Controllers.RoomRequest
{
    [Authorize]
    public class RoomRequestsController : Controller
    {
        /// <summary>
        /// Gets all RoomRequests of a User
        /// </summary>
        /// <returns></returns>
        // GET: RoomRequests
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                if (Check.IsNull(tp, "Index RoomRequests"))
                {

                    return RedirectToAction("Index", "Home");
                }
                using (var db = new ApplicationDbContext())
                {
                    IQueryable<Models.RoomRequest.RoomRequest> IQuery = db.RoomRequests.Where(p => p.ApplyingTraveler_ID == tp.ID).Include(p=>p.RoomRequestResponse);
                    IEnumerable<Models.RoomRequest.RoomRequest> roomRqList = await IQuery.ToListAsync();
                    List<Models.RoomRequest.RoomRequestListUser> resultList = new List<Models.RoomRequest.RoomRequestListUser>();

                    foreach (var rRq in roomRqList)
                    {
                        
                            
                        Models.Request.Request rq = await db.Requests.FindAsync(rRq.Request_ID);
                        rq.Accommodation = await AccommodationsController.LoadAccommodation(rq.Accommodation_ID);
                        Models.VM.ProfilePreviewVM OwnerPrev = await ProfilesController.ProfilePreviewVM(rq.RequestOwner_ID);
                        Models.RoomRequest.RoomRequestListUser rqVM = new Models.RoomRequest.RoomRequestListUser(rRq, rq, OwnerPrev);
                        resultList.Add(rqVM);
                        
                    }
                    return View(resultList);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        public static async Task<string> RoomRequestStats(Models.Request.Request request)
        {

            using (var db = new ApplicationDbContext())
            {
                IQueryable<Models.RoomRequest.RoomRequest> rReqs = db.RoomRequests.Where(p => p.Request_ID == request.ID).Include(p => p.RoomRequestResponse);
                List<Models.RoomRequest.RoomRequest> rReqsPend = await rReqs.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Pending).ToListAsync();
                int numrReqsPend = rReqsPend.Count;
                List<Models.RoomRequest.RoomRequest> rReqsDecl = await rReqs.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Declined).ToListAsync();
                int numrReqsDecl = rReqsDecl.Count;
                List<Models.RoomRequest.RoomRequest> rReqsApprov = await rReqs.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Approved).ToListAsync();
                int numrReqsApprov = rReqsApprov.Count;
                List<Models.RoomRequest.RoomRequest> rReqsPropos = await rReqs.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Proposed).ToListAsync();
                int numrReqsProp = rReqsPropos.Count;
                List<Models.RoomRequest.RoomRequest> rReqsReserv = await rReqs.Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Reserved).ToListAsync();
                int numrReqsRes = rReqsReserv.Count;

                int numrReqs = numrReqsApprov + numrReqsPend + numrReqsProp + numrReqsRes + numrReqsDecl;
                return numrReqs + " / " + numrReqsPend + " / " + numrReqsDecl + " / " + numrReqsApprov + " / " + numrReqsProp + " / " + numrReqsRes;
            }
        }
       
       /// <summary>
       /// returns all non pending requests for approval
       /// </summary>
       /// <param name="id"></param>
       /// <param name="update"></param>
       /// <param name="rrqid"></param>
       /// <returns></returns>

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> SelectFlatmates(int id, int update, int rrqid)  //id RequestID
        {
            try
            {
                

                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);

                
                using (var db = new ApplicationDbContext())
                {

                   
                    Models.Request.Request request = await db.Requests.FindAsync(id);
                    request.Accommodation = await AccommodationsController.LoadAccommodation(request.Accommodation_ID);
                    
                    if (Check.IsNull(request, "UpdateSelectFlatmates"))
                    {
                        return RedirectToAction("Index", "Requests");
                    }
                    if (request.RequestOwner_ID != tp.ID)
                    {
                        NotificationManager.AddException(new Exception("User is not RequestOwner, Access Violation, AcceptTraveler", new AccessViolationException()));
                        return RedirectToAction("Index", "Home");
                    }
                    if (update != 0 && rrqid != 0)
                    {
                        Models.RoomRequest.RoomRequest roomRequest = await db.RoomRequests.FindAsync(rrqid);
                        if (Check.IsNull(roomRequest, "UpdateSelectFlatmates"))
                        {
                            return RedirectToAction("Index", "Requests");
                        }
                        roomRequest.RoomRequestResponse = await db.RoomRequestResponse.FindAsync(roomRequest.RoomRequestResponse_ID);
                        if (roomRequest.IsDeleted("RoomRequest") || request.IsDeleted("RoomRequest"))
                        {
                            NotificationManager.AddNotification(NotificationType.Warning, "RoomRequest is not valid, Request might has been been deleted");
                            return RedirectToAction("Index", "Home");
                        }

                        var trackedRoomRq = roomRequest;
                        var trackedRRqResponse = roomRequest.RoomRequestResponse;

                        if (update > 0)
                        {
                            roomRequest.RoomRequestResponse.Approve();
                            NotificationManager.AddNotification(NotificationType.Success, "Flatmate approved!");
                        }
                        if (update < 0)
                        {
                            roomRequest.RoomRequestResponse.Decline();
                            NotificationManager.AddNotification(NotificationType.Success, "Flatmate declined!");
                        }

                        db.Entry(trackedRoomRq).CurrentValues.SetValues(roomRequest);
                        db.Entry(trackedRRqResponse).CurrentValues.SetValues(trackedRRqResponse);
                        await db.SaveChangesAsync();
                    }
                    
                    IQueryable<Models.RoomRequest.RoomRequest> roomRequestQuery = db.RoomRequests.Where(p => p.Request_ID == id).Include(p => p.RoomRequestResponse).Where(p => p.RoomRequestResponse.responseStatus == Models.RoomRequest.RoomRequestResponseStatus.Pending);
                    List<Models.RoomRequest.RoomRequest> roomRequestList = await roomRequestQuery.ToListAsync();
                    List<Models.RoomRequest.RReqVMListItemRqOwn> roomRequestVMList = new List<Models.RoomRequest.RReqVMListItemRqOwn>();
                    foreach (var roomRq in roomRequestList)
                    {
                        if (roomRq.IsValid())
                        {
                                                        
                            Models.VM.ProfilePreviewVM applicantVMPrev = await ProfilesController.ProfilePreviewVM(roomRq.ApplyingTraveler_ID);
                            roomRequestVMList.Add(new Models.RoomRequest.RReqVMListItemRqOwn(roomRq, request, applicantVMPrev));
                        }
                    }
                    string rrqstats = await RoomRequestStats(request);
                    Models.RoomRequest.RReqVMListRqOwn SelectFM = new Models.RoomRequest.RReqVMListRqOwn(request.ID, roomRequestVMList, rrqstats);
                    
                    return View(SelectFM);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong");
                return RedirectToAction("Index", "Requests");

            }
        }

       
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Propose(int id, int rqid) // id RoomRequestID
        {
            try
            {

                TravelerProfile travelerProfile = await ProfileManager.LoadUserAndTravelerProfile(User);
                using (var db = new ApplicationDbContext())
                {
                    Models.RoomRequest.RoomRequest roomRequest = await db.RoomRequests.FindAsync(id); //test with null!!!
                    if (Check.IsNull(roomRequest, "Propose"))
                    {
                        return RedirectToAction("SelectFlatmates", new { id = rqid, update = 0, rrqid = 0 });
                    }
                   
                    Models.Request.Request request = await db.Requests.FindAsync(roomRequest.Request_ID);
                    if (Check.IsNull(request, "Propose"))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (roomRequest.IsDeleted("Propose") || request.IsDeleted("Propose")) //Todo delete RoomRequest if request is deleted
                    {
                        NotificationManager.AddNotification(NotificationType.Warning, "RoomRequest is not valid, Request might has been deleted");
                        return RedirectToAction("Index", "Home");
                    }
                    if (request.RequestOwner_ID != travelerProfile.ID)
                    {
                        NotificationManager.AddException(new Exception("Access Violation @Propose, RoomRqCtrl", new AccessViolationException()));
                        return RedirectToAction("Index", "Home");
                    }
                    Models.RoomRequest.RoomRequestVMOwner roomRequestVM = new Models.RoomRequest.RoomRequestVMOwner(roomRequest);
                    return View(roomRequestVM);
                    
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return RedirectToAction("SelectFlatmates", new { id = rqid, update = 0, rrqid = 0});
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Propose(Models.RoomRequest.RoomRequestVMOwner roomRqVM) // id RoomRequestID
        {
            try
            {
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                using (var db = new ApplicationDbContext())
                {
                    Models.RoomRequest.RoomRequest roomRequest = await db.RoomRequests.FindAsync(roomRqVM.ID);
                    Models.Request.Request request = await db.Requests.FindAsync(roomRequest.Request_ID);
                    if (request.RequestOwner_ID != tp.ID)
                    {
                        throw new Exception("Access Violation", new AccessViolationException());
                    }
                    roomRequest.RoomRequestResponse = await db.RoomRequestResponse.FindAsync(roomRequest.RoomRequestResponse_ID);
                    Models.RoomRequest.RoomRequest roomRequestUpdate = roomRequest;
                    
                    roomRequestUpdate.SetValues(roomRqVM);
                    db.Entry(roomRequest).CurrentValues.SetValues(roomRequestUpdate);
                    db.Entry(roomRequest.RoomRequestResponse).CurrentValues.SetValues(roomRequestUpdate.RoomRequestResponse);
                    await db.SaveChangesAsync();
                    NotificationManager.AddNotification(NotificationType.Success, "Your proposal has been saved!");
                    return RedirectToAction("SelectFlatmates", new { id = roomRqVM.Request_ID, update = 0, rrqid = 0 });
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong");
                return RedirectToAction("SelectFlatmates", new { id = roomRqVM.Request_ID, update = 0, rrqid = 0 });
            }
        }
        //Todo Details User has to be implemented

        [HttpGet]
        //GET: RoomRequests/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Models.RoomRequest.RoomRequest roomRequest = await db.RoomRequests.FindAsync(id);
                    roomRequest.RoomRequestResponse = await db.RoomRequestResponse.FindAsync(roomRequest.RoomRequestResponse_ID);
                    if (Check.IsNull(roomRequest, "DetailsRoomRq"))
                    {
                        NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong");
                        return View();
                    }
                    Models.Request.Request rq = await db.Requests.FindAsync(roomRequest.Request_ID);
                    rq.Accommodation = await AccommodationsController.LoadAccommodation(rq.Accommodation_ID);
                    Models.VM.ProfilePreviewVM host = await ProfilesController.ProfilePreviewVM(rq.RequestOwner_ID);
                    Models.RoomRequest.RoomRequestVMUser roomRequestVM = new Models.RoomRequest.RoomRequestVMUser(roomRequest, rq, host);
                    return View(roomRequestVM);
                }
            }
            catch (Exception e)
            {

                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong");
                return RedirectToAction("Index", "Home");
            }
        }

        ///Todo details owner has to be implemented
        [HttpGet]
        // GET: RoomRequests/Details/5
        public async Task<ActionResult> DetailsHost(int? id)
        {
            if (Check.IsNull(id))
            {
                return View();
            }
            using (var db = new ApplicationDbContext())
            {
                Models.RoomRequest.RoomRequest roomRequest = await db.RoomRequests.FindAsync(id);
                if (Check.IsNull(roomRequest, "DetailsRoomRqOwner"))
                {
                    return View();
                }
                roomRequest.RoomRequestResponse = await db.RoomRequestResponse.FindAsync(roomRequest.RoomRequestResponse_ID);
                Models.VM.ProfilePreviewVM applPreVM = await ProfilesController.ProfilePreviewVM(roomRequest.ApplyingTraveler_ID);
                Models.RoomRequest.RoomRequestVMOwner roomRequestVM = new Models.RoomRequest.RoomRequestVMOwner(roomRequest, applPreVM);
                return View(roomRequestVM);
            }
        }
        // GET: RoomRequests/Create
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Create(int? id) //Request id
        {
            try
            {
                
                
                if (!id.HasValue)
                {                                        
                    return RedirectToAction("Search", "Searches");
                }
                TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                using (var db = new ApplicationDbContext())
                {
                    TravelerProfileSearchSession searchSession = await db.LikibuSearchSession.FindAsync(tp.TravelerSearchID); //to fill checkDates, Location
                    Models.Request.Request request = await db.Requests.FindAsync(id);
                    request.Accommodation = await AccommodationsController.LoadAccommodation(request.Accommodation_ID);
                    if (Check.IsNull(request, "Create RoomRequest"))
                    {
                        NotificationManager.AddNotification(NotificationType.Warning, "Request is not valid");
                        return RedirectToAction("Search", "Searches");
                    }
                    if (request.IsDeleted("CreateRoomRequest, rq deleted")) 
                    {
                        NotificationManager.AddNotification(NotificationType.Warning, "Request is not valid");
                        return RedirectToAction("Search", "Searches");
                    }
                    if (request.IsClosed()) //dann keine RoomRequests mehr
                    {
                        NotificationManager.AddNotification(NotificationType.Info, "You can't apply for closed requests");
                        return RedirectToAction("Search", "Searches");
                    }
                    //calc Valid Time here
                    DateTime CheckIn = request.CheckIn;
                    DateTime CheckOut = request.CheckOut;
                    DateTime chckinUser = DateTime.Parse(searchSession.CheckIn);
                    DateTime chckoutUser = DateTime.Parse(searchSession.CheckOut);
                    if (chckinUser < CheckIn)
                    {
                        CheckIn = chckinUser;
                    }

                    if (chckoutUser > CheckOut)
                    {
                        CheckOut = chckoutUser;
                    }
                    
                    
                    Models.RoomRequest.RoomRequestVMUser roomRequestVM = new Models.RoomRequest.RoomRequestVMUser(CheckIn, CheckOut, request);
                    if (tp.ID == request.RequestOwner_ID)
                    {
                        roomRequestVM.RequestOwner = true;
                    }
                    return View(roomRequestVM);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        private bool ValidateRoomRq(Models.RoomRequest.RoomRequestVMUser roomRequestVM)
        {
            //ChecktimeValidation, must be here, due to string dates in VM
            DateTime checkinRRq = roomRequestVM.CheckIn;
            DateTime checkoutRRq = roomRequestVM.CheckOut;

            DateTime checkinRq = roomRequestVM.CheckInRequest;
            DateTime checkoutRq = roomRequestVM.CheckOutRequest;
            bool checkinFail = false;
            if (checkinRRq < checkinRq || checkinRRq > checkoutRq)
            {
                ModelState.AddModelError("CheckIn", "CheckIn must be in valid requesttime");
                checkinFail = true;
            }

            if (checkoutRRq < checkinRq || checkoutRRq > checkoutRq)
            {
                ModelState.AddModelError("CheckOut", "CheckOut must be in valid requesttime");
                checkinFail = true;
            }
            if (!roomRequestVM.DoubleBed && !roomRequestVM.SingleBed && !roomRequestVM.Nevermind)
            {
                ModelState.AddModelError("DoubleBed", "Please select a bed type");
                checkinFail = true;

            }
            if (roomRequestVM.Price == null || Models.Request.Request.Price(roomRequestVM.Price) == 0)
            {
                ModelState.AddModelError("Price", "Please insert a price!");
                checkinFail = true;

            }
            return checkinFail;

        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.RoomRequest.RoomRequestVMUser roomRequestVM) 
        {
            if (roomRequestVM.RequestOwner)
            {
                ModelState.Remove("Text");
                ModelState.Remove("CheckIn");
                ModelState.Remove("CheckOut");
                
            }
            ModelState.Remove("Price");
            if (ModelState.IsValid)
            {
                if (roomRequestVM.RequestOwner)
                {
                    if ((!roomRequestVM.SingleBed && !roomRequestVM.DoubleBed)) //No beds needed
                    {
                        return RedirectToAction("Index", "Requests");
                    }

                }
                if (!roomRequestVM.RequestOwner)
                {
                    if (ValidateRoomRq(roomRequestVM))
                    {
                        return View(roomRequestVM);
                    }
                }
                
                try
                {

                    TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                    
                    using (var db = new ApplicationDbContext())
                    {
                        ///Save new RoomRequest 
                        Models.RoomRequest.RoomRequest roomRequest = new Models.RoomRequest.RoomRequest(roomRequestVM);
                        roomRequest.CheckInOwner = roomRequest.CheckInUser;
                        roomRequest.CheckOutOwner = roomRequest.CheckOutUser;
                        roomRequest.ApplyingTraveler_ID = tp.ID;
                        roomRequest.RoomRequestResponse = new Models.RoomRequest.RoomRequestResponse();
                        roomRequest.RoomRequestResponse.Init();
                        if (roomRequestVM.RequestOwner) //RequestOwner requests a room
                        {                            
                            roomRequest.RoomRequestResponse.Approve();
                        }
                        
                        db.RoomRequests.Add(roomRequest);
                        
                        await db.SaveChangesAsync();
                        
                        NotificationManager.AddNotification(NotificationType.Success, "You have successfully applied for a room!");
                        if (roomRequestVM.RequestOwner) { return RedirectToAction("Index", "Requests"); }
                        

                        return RedirectToAction("Search", "Searches"); //Todo: Search has to redirected to action with parameters
                    }
                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return View(roomRequestVM);
        }
        /// <summary>
        /// Todo Exception handling // Show RoomRequestResponse in View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: RoomRequests/Edit/5 ///
        [HttpGet]
        public async Task<ActionResult> Edit(int? id) //id roomRq
        {
            try
            {
                if (!id.HasValue)
                {

                    return RedirectToAction("Search", "Searches");
                }
                TravelerProfile travelerProfile = await ProfileManager.LoadUserAndTravelerProfile(User);
                using (var db = new ApplicationDbContext())
                {
                    Models.RoomRequest.RoomRequest roomRequest = await db.RoomRequests.FindAsync(id);
                    if (Check.IsNull(roomRequest, "Edit roomRequest"))
                    {
                        return RedirectToAction("Index");
                    }
                    Models.Request.Request request = await db.Requests.FindAsync(roomRequest.Request_ID);
                    request.Accommodation = await AccommodationsController.LoadAccommodation(request.Accommodation_ID);

                    if (!request.IsValid())
                    {

                        NotificationManager.AddNotification(NotificationType.Info, "The request is invalid, you can't edit your roomrequest anymore");
                        return RedirectToAction("Index");
                    }
                    if (roomRequest.ApplyingTraveler_ID != travelerProfile.ID)
                    {
                        NotificationManager.AddException(new Exception("Access violation, this is not your Roomrequest! Edit RoomRequest", new AccessViolationException()));

                        return RedirectToAction("Index");
                    }
                    roomRequest.RoomRequestResponse = db.RoomRequestResponse.Find(roomRequest.RoomRequestResponse_ID);

                    if (roomRequest.RoomRequestResponse.IsApproved())
                    {
                        NotificationManager.AddNotification(NotificationType.Info, "You cannot edit an approved roomrequest!");
                        return RedirectToAction("Index");
                    }
                    if (roomRequest.RoomRequestResponse.IsReserved())
                    {
                        NotificationManager.AddNotification(NotificationType.Info, "You cannot edit a reserved roomrequest!");
                        return RedirectToAction("Index");
                    }
                    Models.VM.ProfilePreviewVM host = await ProfilesController.ProfilePreviewVM(request.RequestOwner_ID);
                    Models.RoomRequest.RoomRequestVMUser roomRequestEditVM = new Models.RoomRequest.RoomRequestVMUser(roomRequest, request, null);

                    return View(roomRequestEditVM);
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                return RedirectToAction("Index");
            }
        }

        // POST: RoomRequests/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.RoomRequest.RoomRequestVMUser roomRequestVM, string accept, string save)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new ApplicationDbContext())
                    {


                        var trackedRoomRq = await db.RoomRequests.FindAsync(roomRequestVM.ID);
                        trackedRoomRq.RoomRequestResponse = await db.RoomRequestResponse.FindAsync(trackedRoomRq.RoomRequestResponse_ID);
                        Models.RoomRequest.RoomRequest rRqUpdate = trackedRoomRq;
                        bool update = false;
                        if (!string.IsNullOrEmpty(accept))
                        {
                            rRqUpdate.AcceptProposal();
                            update = true;
                        }
                        if (!string.IsNullOrEmpty(save))
                        {
                            rRqUpdate.SetValues(roomRequestVM);
                            update = true;
                        }
                        if (update)
                        {
                            db.Entry(trackedRoomRq).CurrentValues.SetValues(rRqUpdate);
                            db.Entry(trackedRoomRq.RoomRequestResponse).CurrentValues.SetValues(rRqUpdate.RoomRequestResponse);
                            await db.SaveChangesAsync();
                        }
                        return RedirectToAction("Index");
                    }



                }
                catch (Exception e)
                {
                    NotificationManager.AddException(e);
                    NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                }

            }
            return View(roomRequestVM);
        }

        // GET: RoomRequests/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (Check.IsNull(id))
            {
                return RedirectToAction("Index");
            }
            TravelerProfile travelerProfile = await ProfileManager.LoadUserAndTravelerProfile(User);
            using (var db = new ApplicationDbContext())
            {
                Models.RoomRequest.RoomRequest roomRequest = db.RoomRequests.Find(id);
                roomRequest.RoomRequestResponse = db.RoomRequestResponse.Find(roomRequest.RoomRequestResponse_ID);
                if (Check.IsNull(roomRequest,"RoomRequest Delete"))
                {
                    return RedirectToAction("Index");
                }
                if (roomRequest.ApplyingTraveler_ID != travelerProfile.ID)
                {
                    NotificationManager.AddException(new Exception("AccessViolation Roomrq Delete"));
                    return RedirectToAction("Index");
                }
                Models.Request.Request rq = await db.Requests.FindAsync(roomRequest.Request_ID);
                rq.Accommodation = await AccommodationsController.LoadAccommodation(rq.Accommodation_ID);
                Models.VM.ProfilePreviewVM host = await ProfilesController.ProfilePreviewVM(rq.RequestOwner_ID);
                Models.RoomRequest.RoomRequestVMUser roomRequestVM = new Models.RoomRequest.RoomRequestVMUser(roomRequest, rq, host);
                return View(roomRequestVM);
            }
        }

        // POST: RoomRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Models.RoomRequest.RoomRequest roomRequest = db.RoomRequests.Find(id);
                    Models.RoomRequest.RoomRequestResponse response = db.RoomRequestResponse.Find(roomRequest.RoomRequestResponse_ID);
                    roomRequest.Delete();
                    response.Canceled();
                    db.Entry(response).State = EntityState.Modified;
                    db.Entry(roomRequest).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    NotificationManager.AddNotification(NotificationType.Success, "Room request has been deleted!");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                NotificationManager.AddException(e);
                NotificationManager.AddNotification(NotificationType.Error, "Oops, something went wrong!");
                return RedirectToAction("Index");
            }
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

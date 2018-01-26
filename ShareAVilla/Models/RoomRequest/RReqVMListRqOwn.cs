using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models.RoomRequest
{
    //
    public class RReqVMListRqOwn
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public List<RReqVMListItemRqOwn> RoomRequests { get; set; }
        [Display(Name = "All/Pending/Declined/Approved/Proposed/Reserved:")]
        public string RoomRequestsStats { get; set; } //Pending, Approved, Proposed, Reserved  
        public RReqVMListRqOwn(int rqID, List<RReqVMListItemRqOwn> rRqList, string stats)
        {
            RequestID = rqID;
            RoomRequests = rRqList;
            RoomRequestsStats = stats;
        }
    }
    public class RReqVMListItemRqOwn
    {
        public int ID { get; set; }
        //public string Thumb { get; set; }
        [Display(Name = "Requested CheckIn: ")]
        public string CheckIn { get; set; }
        [Display(Name = "Requested CheckOut: ")]
        public string CheckOut { get; set; }
        [Display(Name = "Price by User: ")]
        public string TravelerPrice { get; set; }
        [Display(Name = "Appliying User: ")]
        public VM.ProfilePreviewVM Flatmate { get; set; }
        [Display(Name = "Requested Room")]
        public string RoomType { get; set; }
        [Display(Name ="Status ")]
        public string Status { get; set; }
        [Display(Name = "Calculated Price: ")]
        public string CalcPrice { get; set; }
        [Display(Name = "Calculated Sales Price: ")]
        public string CalcPriceSales { get; set; }

        [Display(Name = "Price range: ")]
        public string PriceRange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rRq"></param>
        /// <param name="rq"></param>
        public RReqVMListItemRqOwn(RoomRequest rRq, Request.Request rq, VM.ProfilePreviewVM profileUser)
        {
            ID = rRq.ID;
            CheckIn = rRq.CheckInUser.ToString("dd.MM.yyyy");
            CheckOut = rRq.CheckOutUser.ToString("dd.MM.yyyy");
            TravelerPrice = rRq.PriceUser;
            RoomType = rRq.RoomTypeToString();
            Status = rRq.RoomRequestResponse.StatusString;
            Flatmate = profileUser;
            if (rq.Type == Request.RequestType.Bound)
            {
                CalcPrice = Request.Request.PriceCurrency(Request.Request.CalcPrice(rRq.CheckInUser, rRq.CheckOutUser, Request.Request.Price(rq.Accommodation.AccomProfile.PricePNight), Request.Request.Price(rq.Accommodation.AccomProfile.PricePWeek), Request.Request.Price(rq.Accommodation.AccomProfile.PricepMonth), rq.Accommodation.AccomProfile.BedRooms).ToString());
                CalcPriceSales = Request.Request.PriceCurrency(Request.Request.CalcPrice(rRq.CheckInUser, rRq.CheckOutUser, Request.Request.Price(rq.SalesPricePDay), Request.Request.Price(rq.SalesPricePWeek), Request.Request.Price(rq.SalesPricePMonth), 1).ToString());
            }
            if (rq.Type == Request.RequestType.Free)
            {
                PriceRange = rq.PriceMin+" - "+rq.PriceMax;

            }
            //Thumbs = Accommodation.Likibu.Offer.StringToArray(rq.Accommodation.AccomProfile.Thumbnails).ToList().FirstOrDefault();
           
        }
    }
}
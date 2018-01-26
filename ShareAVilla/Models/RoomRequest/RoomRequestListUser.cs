using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.RoomRequest
{
    public class RoomRequestListUser
    {
        public int ID { get; set; }
        public string Thumb { get; set; }
        [Display(Name = "Your CheckIn: ")]
        [DataType(DataType.Date)]
        public string CheckIn { get; set; }   
        [Display(Name = "Your CheckOut: ")]
        [DataType(DataType.Date)]
        public string CheckOut { get; set; }       
        [Display(Name = "Your Price: ")]
        public string Price { get; set; }               
        [Display(Name = "Price/room/time: ")]
        public string CalcPrice { get; set; }       
        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }
        [Display(Name = "Room type: ")]
        public string Roomtype { get; set; }
        public string Title { get; set; }          
        public int Request_ID { get; set; }//
        public VM.ProfilePreviewVM RqOwnerPrev { get; set; }
        [Display(Name = "Status: ")]
        public string Status { get; set; }
        public RoomRequestResponseStatus Rstate { get; set; }
        public RoomRequestListUser(RoomRequest rRq, Request.Request rq, VM.ProfilePreviewVM rqOwnerPrev)
        {
            ID = rRq.ID;
            Request_ID = rq.ID;
            CheckIn = rRq.CheckInUser.ToString("dd.MM.yyyy");
            CheckOut = rRq.CheckOutUser.ToString("dd.MM.yyyy");
            string cur = "";
            if (rq.Type == Request.RequestType.Bound)
            {
                cur = Request.Request.GetCurrency(rq.SalesPricePDay);
                
                CalcPrice = Request.Request.CalcPrice(rRq.CheckInUser, rRq.CheckOutUser, Request.Request.Price(rq.SalesPricePDay), Request.Request.Price(rq.SalesPricePWeek), Request.Request.Price(rq.SalesPricePMonth), 1).ToString() + cur;
            }
            Title = rq.Title;
            Price = rRq.PriceUser;
            Thumb = Accommodation.Likibu.Offer.StringToArray(rq.Accommodation.AccomProfile.Thumbnails).FirstOrDefault();
            Roomtype = rRq.RoomType.ToString();
            BedRooms = rq.Accommodation.AccomProfile.BedRooms;
            RqOwnerPrev = rqOwnerPrev;
            Status = rRq.RoomRequestResponse.StatusString;
            Rstate = rRq.RoomRequestResponse.responseStatus;
        }
    }
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models.RoomRequest
{
    public class RoomRequestVMUser
    {
        public int ID { get; set; }
        public VM.ProfilePreviewVM Host { get; set; }
        [Display(Name = "Valid from: ")]
        [DataType(DataType.Date)]
        public DateTime CheckInRequest { get; set; }
        [Display(Name = "till: ")]
        [DataType(DataType.Date)]
        public DateTime CheckOutRequest { get; set; }
        [Display(Name = "Proposed CheckIn: ")]
        [DataType(DataType.Date)]
        public DateTime CheckInOwner { get; set; }
        [Display(Name = "Proposed CheckOut: ")]
        [DataType(DataType.Date)]
        public DateTime CheckOutOwner { get; set; }
        [Required]
        [Display(Name = "Your CheckIn: ")]
        [DataType(DataType.Date)]       
        public DateTime CheckIn { get; set; }
        [Required]
        [Display(Name = "Your CheckOut: ")]
        [DataType(DataType.Date)]       
        public DateTime CheckOut { get; set; }
        [Required]
        [Display(Name = "Your price:")]
        public string Price { get; set; }
        [Display(Name = "Price by host:")]
        public string PriceOwner { get; set; }
        [Display(Name = "Message: ")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        [Display(Name = "Price/\nroom/night: ")]
        public string PricePNigthPRoom { get; set; }
        [Display(Name = "Price/\nroom/week: ")]
        public string PricePWeekPRoom { get; set; }
        [Display(Name = "Price/\nroom/month: ")]        
        public string PricePMonthPRoom { get; set; }
        [Display(Name = "Price/\nroom/time: ")]
        public string PricePRoomPTime { get; set; }

        [Display(Name = "Price range: ")]
        public string PriceSpan { get; set; }


        [Display(Name = "Room Type: ")]
        public string Roomtype { get; set; }
        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }
        [Display(Name = "Share Room:")]        
        public bool ShareRoom { get; set; }
        [Display(Name = "Kingsizebed:")]
        public bool DoubleBed { get; set; }
        [Display(Name = "Singlebed:")]
        public bool SingleBed { get; set; }
        
        [Display(Name = "Share Room:")]
        public bool ShareRoomOwner { get; set; }
        [Display(Name = "Kingsizebed:")]
        public bool DoubleBedOwner { get; set; }
        [Display(Name = "Singlebed:")]
        public bool SingleBedOwner { get; set; }
        [Display(Name = "Nevermind:")]
        public bool Nevermind { get; set; }
        public Request.RequestType RqType { get; set; }
        [Display(Name = "Would you like a room? Check or leave blank!")]
        public bool RequestOwner { get; set; }
        public int Request_ID { get; set; }//
        public int RoomRqID { get; set; }
        public RoomRequestResponseStatus Status { get; set; }
        /// <summary>
        /// this goes to the View When RoomRequest is created(when applying for a request)
        /// </summary>
        /// <param name="checkIn"></param>
        /// <param name="checkOut"></param>
        
        public RoomRequestVMUser(DateTime checkin, DateTime checkout, Request.Request rq ) {

            CheckInRequest = rq.CheckIn;
            CheckOutRequest = rq.CheckOut;
            CheckIn = checkin;
            CheckOut = checkout;
            RqType = rq.Type;
            
            if (rq.Type == Request.RequestType.Bound)
            {

                string cur = Request.Request.GetCurrency(rq.SalesPricePDay);
                PricePNigthPRoom = (rq.SalesPricePDay); //SalesPrices in rq are per Room
                PricePWeekPRoom = (rq.SalesPricePWeek);
                PricePMonthPRoom = (rq.SalesPricePMonth);
                PricePRoomPTime = (Request.Request.CalcPrice(checkin, checkout, Request.Request.Price(PricePNigthPRoom), Request.Request.Price(PricePWeekPRoom), Request.Request.Price(PricePMonthPRoom), 1).ToString());
            }
            if (rq.Type == Request.RequestType.Free)
            {
                PriceSpan = Request.Request.PriceCurrency(rq.PriceMax) + "-"+ Request.Request.PriceCurrency(rq.PriceMin);
                
            }
            Request_ID = rq.ID;
            BedRooms = rq.Accommodation.AccomProfile.BedRooms;
           

        }
        public RoomRequestVMUser() { }

        /// <summary>
        /// When RoomRequest is loaded from DB
        /// </summary>
        /// <param name="rRq"></param>
        public RoomRequestVMUser(RoomRequest rRq, Request.Request rq, VM.ProfilePreviewVM host) // when it comes from DB to view
        {
            ID = rRq.ID;
            Host = host;
            CheckInRequest = rq.CheckIn;
            CheckOutRequest = rq.CheckOut;
            CheckInOwner = rRq.CheckInOwner;
            CheckOutOwner = rRq.CheckOutOwner;
            CheckIn = rRq.CheckInUser;
            CheckOut = rRq.CheckOutUser;
            string cur = "$";//currency has to be set by user

            if (rq.Type == Request.RequestType.Bound)
            {
                cur = Request.Request.GetCurrency(rq.SalesPricePDay);
                PricePNigthPRoom = (rq.SalesPricePDay); //SalesPrices in rq are per Room
                PricePWeekPRoom = (rq.SalesPricePWeek);
                PricePMonthPRoom = (rq.SalesPricePMonth);
                cur = Request.Request.GetCurrency(rq.SalesPricePDay);
                PricePRoomPTime = (Request.Request.CalcPrice(rRq.CheckInUser, rRq.CheckOutUser, Request.Request.Price(PricePNigthPRoom), Request.Request.Price(PricePWeekPRoom), Request.Request.Price(PricePMonthPRoom), 1).ToString("0.00")+cur);
            }
            if (rq.Type == Request.RequestType.Free)
            {
                cur = Request.Request.GetCurrency(rq.PriceMax);
                PriceSpan = (rq.PriceMax) + "-" + (rq.PriceMin);
                
            }
            if (rRq.PriceOwner != null)
            {
                PriceOwner = Request.Request.PriceCurrency(rRq.PriceOwner, cur);
            }
            if (rRq.PriceUser != null)
            {
                Price = Request.Request.PriceCurrency(rRq.PriceUser, cur);
            }
            SingleBedOwner = rRq.SingleBedPropose > 0;
            DoubleBedOwner = rRq.DoubleBedPropose > 0;
            ShareRoomOwner = rRq.ShareRoomPropose;
            Status = rRq.RoomRequestResponse.responseStatus;
            BedRooms = rq.Accommodation.AccomProfile.BedRooms;
            Roomtype = rRq.RoomType.ToString();
            RoomRqID = rRq.ID; //not needed
            
            Text = rRq.Message;
            ShareRoom = rRq.ShareRoom;
            DoubleBed = rRq.DoubleBed > 0;
            SingleBed = rRq.SingleBed > 0;
            Request_ID = rRq.Request_ID;
            RqType = rq.Type;
            //available rooms calc in controller...
        }

    }
}

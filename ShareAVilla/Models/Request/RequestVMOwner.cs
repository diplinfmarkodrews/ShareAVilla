using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.Request
{
    public class RequestVMOwner
    {
        public int ID { get; set; }
        [Display(Name = "CheckIn: ")]
        [Required]
        public DateTime CheckIn { get; set; }
        [Display(Name = "CheckOut: ")]
        [Required]
        public DateTime CheckOut { get; set; }
        [Display(Name = "Location: ")]
        public string Location { get; set; }
        public List<string> Thumbs { get; set; }
        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }
        [Display(Name = "MaxGuests: ")]
        public int MaxGuests { get; set; }
        [Display(Name = "Vendor Price/\nNight: ")]
        public string PricePNight { get; set; }
        [Display(Name = "Vendor Price/\nWeek: ")]
        public string PricePWeek { get; set; }
        [Display(Name = "Vendor Price/\nMonth: ")]
        public string PricepMonth { get; set; }
        [Display(Name = "Sales Price/\nNight/Room: ")]
        [Required]
        public string SalesPricePNight { get; set; }
        [Display(Name = "Sales Price/\nWeek/Room: ")]
        [Required]
        public string SalesPricePWeek { get; set; }
        [Display(Name = "Sales Price/\nMonth/Room: ")]
        [Required]
        public string SalesPricepMonth { get; set; }
        [Display(Name = "Title: ")]
        [DataType(DataType.Text)]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Text: ")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Text { get; set; }
        [Display(Name ="Vendor Price/Room/\nRequested Time:")]
        public string PricePRoomPerTime { get; set; }
        [Display(Name = "Total Vendor Price/\nRequested Time:")]
        public string TotalPricePRq { get; set; }
        [Display(Name = "Max Price/Room:")]
        public string MaxPrice { get; set; }
        [Display(Name = "Min Price/Room:")]
        public string MinPrice { get; set; }
        [Display(Name = "Price range:")]
        public string PriceRange { get; set; }
        [Display(Name = "All/Pending/Declined/Approved/Proposed/Reserved:")]
        public string RoomRequests { get; set; } //Pending, Declined, Approved, Proposed, Reserved     
        
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string AccomID { get; set; }
        [Display(Name = "URL:")]
        public string Link { get; set; }
        public RequestType requestType { get; set; }
        
        public string ThumbsTemp { get; set; }

            /// <summary>
            /// init create
            /// </summary>
            /// <param name="location"></param>
            /// <param name="bedR"></param>
            /// <param name="ppn"></param>
            /// <param name="ppw"></param>
            /// <param name="ppm"></param>
            /// <param name="lat"></param>
            /// <param name="lng"></param>
            /// <param name="chckin"></param>
            /// <param name="chckout"></param>
            /// <param name="id"></param>
        public RequestVMOwner(string location, int bedR, float ppn, float ppw, float ppm, float lat, float lng, string chckin, string chckout, string id, string cur, string thumbs)
        {
            Location = location;
            BedRooms = bedR;

            cur = Request.CurrencySign(cur);
            CheckIn = DateTime.Parse(chckin);
            CheckOut = DateTime.Parse(chckout);
            AccomID = id;
            requestType = RequestType.Bound;
            PricePNight = ppn.ToString("0.00")+cur;
            PricePWeek = ppw.ToString("0.00") + cur;
            PricepMonth = ppm.ToString("0.00") + cur;

            SalesPricePNight = (ppn/BedRooms).ToString("0.00") + cur;
            SalesPricePWeek = (ppw/BedRooms).ToString("0.00") + cur;
            SalesPricepMonth = (ppm/BedRooms).ToString("0.00") + cur;

            TotalPricePRq = Request.CalcPrice(CheckIn, CheckOut, ppn, ppw, ppm, 1).ToString("0.00") +cur;
            PricePRoomPerTime = Request.CalcPrice(CheckIn, CheckOut, ppn, ppw, ppm, BedRooms).ToString("0.00") +cur;
            Lat = lat;
            Lng = lng;
            Thumbs = Accommodation.Likibu.Offer.StringToArray(thumbs).ToList();
            ThumbsTemp = thumbs;
        }
        /// <summary>
        /// Update Request
        /// </summary>
        /// <param name="request"></param>
        public RequestVMOwner(Request request)
        {
            ID = request.ID;
            Location = request.Accommodation.AccomProfile.Location;
            BedRooms = request.Accommodation.AccomProfile.BedRooms;
            CheckIn = request.CheckIn;
            CheckOut = request.CheckOut;
            requestType = request.Type;
            if (request.Type == RequestType.Bound)
            {
                PricePNight = (request.Accommodation.AccomProfile.PricePNight);
                PricePWeek = (request.Accommodation.AccomProfile.PricePWeek);
                PricepMonth = (request.Accommodation.AccomProfile.PricepMonth);
                SalesPricePNight = (request.SalesPricePDay);
                SalesPricePWeek = (request.SalesPricePWeek);
                SalesPricepMonth = (request.SalesPricePMonth);
                string cur = Request.GetCurrency(PricePNight);
                TotalPricePRq = (Request.CalcPrice(request.CheckIn, request.CheckOut, Request.Price(PricePNight), Request.Price(PricePWeek), Request.Price(PricepMonth), 1).ToString())+cur;
                PricePRoomPerTime = Request.CalcPrice(request.CheckIn, request.CheckOut, Request.Price(SalesPricePNight), Request.Price(SalesPricePWeek), Request.Price(SalesPricepMonth), BedRooms).ToString()+cur;
                AccomID = request.Accommodation.AccomProfile.AccomID;
            }
            if (request.Type == RequestType.Free)
            {
                MaxPrice = (request.PriceMax);
                MinPrice = (request.PriceMin);
                PriceRange = MinPrice + " - " + MaxPrice;
            }
            Link = request.Accommodation.AccomProfile.URL;
            Text = request.Text;
            Title = request.Title;
            Lat = request.Accommodation.AccomProfile.Lat;//editable in free
            Lng = request.Accommodation.AccomProfile.Lng;//
            Thumbs = Accommodation.Likibu.Offer.StringToArray(request.Accommodation.AccomProfile.Thumbnails).ToList();
            Link = request.Accommodation.AccomProfile.URL;//
        }
        public RequestVMOwner(string checkIn, string checkOut)
        {
            CheckIn = DateTime.Parse(checkIn);
            CheckOut = DateTime.Parse(checkOut);
        }

        /// <summary>
        /// DetailsOwner used for creating the request
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="request"></param>
        public RequestVMOwner(string destination, Request request)
        {
            ID = request.ID;

            Location = destination;
            BedRooms = request.Accommodation.AccomProfile.BedRooms;
            CheckIn = request.CheckIn;
            CheckOut = request.CheckOut;
            AccomID = request.Accommodation.AccomProfile.AccomID;
            PricePNight = (request.Accommodation.AccomProfile.PricePNight);
            PricePWeek = (request.Accommodation.AccomProfile.PricePWeek);
            PricepMonth = (request.Accommodation.AccomProfile.PricepMonth);
            Text = request.Text;
            Title = request.Title;
            Lat = request.Accommodation.AccomProfile.Lat;
            Lng = request.Accommodation.AccomProfile.Lng;
            
        }
        public RequestVMOwner() { }

        public RequestVMOwner(Request rq, string rRqStats)
        {
            ID = rq.ID;
            Title = rq.Title;
            Text = rq.Text;
            requestType = rq.Type;
            Location = rq.Accommodation.AccomProfile.Location;
            BedRooms = rq.Accommodation.AccomProfile.BedRooms;
            MaxGuests = rq.Accommodation.AccomProfile.MaxGuests;
            CheckIn = rq.CheckIn; 
            CheckOut = rq.CheckOut;
            Thumbs = Accommodation.Likibu.Offer.StringToArray(rq.Accommodation.AccomProfile.Thumbnails).ToList();
            if (rq.Type == RequestType.Bound)
            {
                PricePNight = (rq.Accommodation.AccomProfile.PricePNight);
                PricePWeek = rq.Accommodation.AccomProfile.PricePWeek;
                PricepMonth = rq.Accommodation.AccomProfile.PricepMonth;
                string cur = Request.GetCurrency(PricePNight);
                SalesPricePNight = rq.SalesPricePDay;
                SalesPricePWeek = rq.SalesPricePWeek;
                SalesPricepMonth = rq.SalesPricePMonth;
                TotalPricePRq = Request.CalcPrice(CheckIn, CheckOut, Request.Price(PricePNight), Request.Price(PricePWeek), Request.Price(PricepMonth), 1).ToString()+cur;
                PricePRoomPerTime = Request.CalcPrice(CheckIn, CheckOut, Request.Price(SalesPricePNight), Request.Price(SalesPricePWeek), Request.Price(SalesPricepMonth), BedRooms).ToString()+cur;
            }
            if (rq.Type == RequestType.Free)
            {
                MaxPrice = Request.PriceCurrency(rq.PriceMax);
                MinPrice = Request.PriceCurrency(rq.PriceMin);
                PriceRange = MinPrice + " - " + MaxPrice;
            }
            RoomRequests = rRqStats;
           
            Lat = rq.Accommodation.AccomProfile.Lat;
            Lng = rq.Accommodation.AccomProfile.Lng;
            Link = rq.Accommodation.AccomProfile.URL;
        }
    }
}
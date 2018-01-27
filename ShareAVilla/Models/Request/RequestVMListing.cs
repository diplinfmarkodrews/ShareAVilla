using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.Request
{
    public class RequestVMListing
    {
        public int ID { get; set; }
        public VM.ProfilePreviewVM Host { get; set; }
        [Display(Name = "Valid from: ")]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }
        [Display(Name = "Valid till: ")]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        [Display(Name = "Location: ")]
        public string Destination { get; set; }
        [Display(Name = "Title: ")]
        public string Title { get; set; }
        [Display(Name = "Price: ")]
        public string PricePNightPRoom { get; set; }

        [Display(Name = "Reservations:")]
        public string RoomRequests { get; set; } //Pending, Declined, Approved, Proposed, Reserved   
        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }
        public string Thumb { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public RequestVMListing(Request request, string roomRqStats, VM.ProfilePreviewVM host)
        {
            ID = request.ID;
            CheckIn = request.CheckIn;
            CheckOut = request.CheckOut;
            Destination = request.Accommodation.AccomProfile.Location;
            Title = request.Title;
            if (request.Type == RequestType.Bound)
            {
              PricePNightPRoom = request.SalesPricePDay;
            }
            if (request.Type == RequestType.Free)
            {
                PricePNightPRoom = request.PriceMin +  " - " + request.PriceMax;
            }
            BedRooms = request.Accommodation.AccomProfile.BedRooms;
            Thumb = Accommodation.Likibu.Offer.StringToArray(request.Accommodation.AccomProfile.Thumbnails).FirstOrDefault();
            Lat = request.Accommodation.AccomProfile.Lat;
            Lng = request.Accommodation.AccomProfile.Lng;
            RoomRequests = roomRqStats;
            Host = host;
        }
    }
}
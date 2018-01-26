using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models.Request
{
    public class RequestVMUser
    {
        public int ID { get; set; }
        [Display(Name = "Valid from: ")]
        public string CheckIn { get; set; }
        [Display(Name = "till: ")]
        public string CheckOut { get; set; }
        [Display(Name = "Location: ")]
        public string Location { get; set; }
        [Display(Name = "Title: ")]
        public string Title { get; set; }
        [Display(Name = "Text: ")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }

        // For bound requests

        [Display(Name = "Price/day/room: ")]
        public string PricePDay { get; set; }
        [Display(Name = "Price/week/room: ")]
        public string PricePWeek { get; set; }
        [Display(Name = "Price/month/room: ")]
        public string PricePMonth { get; set; }
        [Display(Name = "Price/time/room: ")]
        public string TotalPrice { get; set; }
        //free request
        [Display(Name = "Max price/room: ")]
        public string PriceMax { get; set; }
        [Display(Name = "Min price/room: ")]
        public string PriceMin { get; set; }

        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }
        [Display(Name = "Max Guests: ")]
        public int MaxGuests { get; set; }
        [Display(Name = "Host: ")]
        public VM.ProfilePreviewVM RQOwner { get; set; }
        public List<string> Thumb { get; set; }
        public string URL { get; set; }
        [Display(Name = "Reservations: ")]
        public string Reservations { get; set; }

        public RequestType requestType { get; set; }

        public RequestVMUser(Request request, VM.ProfilePreviewVM Owner)
        {
            ID = request.ID;
            requestType = request.Type;
            RQOwner = Owner;
            CheckIn = request.CheckIn.ToString("dd.MM.yyyy");
            CheckOut = request.CheckOut.ToString("dd.MM.yyyy");
            if (request.Type == RequestType.Bound)
            {
               
                
                PricePDay = request.SalesPricePDay;
                string cur = Request.GetCurrency(PricePDay);
                PricePWeek = request.SalesPricePWeek;
                PricePMonth = request.SalesPricePMonth;
                TotalPrice = Request.CalcPrice(DateTime.Parse(CheckIn), DateTime.Parse(CheckOut), Request.Price(request.SalesPricePDay), Request.Price(request.SalesPricePWeek), Request.Price(request.SalesPricePMonth), 1).ToString()+cur;
            }
            if (request.Type == RequestType.Free)
            {
                PriceMax = request.PriceMax;
                PriceMin = request.PriceMin;
            }
            BedRooms = request.Accommodation.AccomProfile.BedRooms;
            MaxGuests = request.Accommodation.AccomProfile.MaxGuests;
            Lat = request.Accommodation.AccomProfile.Lat;
            Lng = request.Accommodation.AccomProfile.Lng;
            Location = request.Accommodation.AccomProfile.Location;
            Title = request.Title;
            Text = request.Text;
            Thumb = Accommodation.Likibu.Offer.StringToArray(request.Accommodation.AccomProfile.Thumbnails).ToList();
            URL = request.Accommodation.AccomProfile.URL;
        }
    }
}
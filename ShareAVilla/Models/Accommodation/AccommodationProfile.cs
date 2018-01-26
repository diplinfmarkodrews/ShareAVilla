using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareAVilla.Models.Accommodation
{
    public enum AccommodationType
    {
        None,
        Privat,
        Likibu,
        AirBnb,

    };
    public class AccommodationProfile:BaseDataObject
    {
        public AccommodationType Type { get; set; }
        public string AType { get; set; }
        public string DestinationIDs { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Thumbnails { get; set; }
        public string Photos { get; set; }
        public int BedRooms { get; set; }
        public int BathRooms { get; set; }
        public int MaxGuests { get; set; }
        public string AccomID { get; set; }
        public string PricePNight { get; set; } 
        public string PricePWeek { get; set; } 
        public string PricepMonth { get; set; } 
        
        public string Location { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string URL { get; set; }
        public AccommodationProfile()
        {
        //    //LikibuOffer = new Likibu.Offer();
        }
        public AccommodationProfile(Likibu.Offer offer, string cur)
        {
            Type = AccommodationType.Likibu;

            AType = offer.Type;
            DestinationIDs = offer.DestinationIDs;
            BedRooms = offer.BedRooms;
            AccomID = offer.offerID;
            PricePNight = offer.PriceNight.ToString()+cur;
            PricePWeek = offer.PriceWeek.ToString()+cur;
            PricepMonth = offer.PriceMonth.ToString()+cur;
            Location = offer.Destination;
            Thumbnails = Likibu.Offer.ArrayToString(offer.Thumbnails.ToArray());
            Photos = Likibu.Offer.ArrayToString(offer.Photos.ToArray());
            Lat = offer.Lat;
            Lng = offer.Lng;
            URL = offer.URL;
            MaxGuests = offer.MaxGuests;
            Title = offer.Title;
            Text = offer.Description;
        }
        public AccommodationProfile(Likibu.Destination dest, int bedrooms, string url)
        {
            Type = AccommodationType.None;
            Location = dest.Name;
            DestinationIDs = dest.ID;
            BedRooms = bedrooms;
            MaxGuests = bedrooms * 2;
            URL = url;
            Thumbnails = "https://upload.wikimedia.org/wikipedia/commons/e/e1/Tokyoship_Home_icon.svg";
        }
    }
   
}
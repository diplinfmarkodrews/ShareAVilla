using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.Request
{
    public class RequestVMFree
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "CheckIn: ")]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }
        [Required]
        [Display(Name = "CheckOut: ")]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        [Required]
        [Display(Name = "Location: ")]
        public string Location { get; set; }
        [Display(Name = "Latitude (if known): ")]
        public float? Lat { get; set; }
        [Display(Name = "Longitude (if known): ")]
        public float? Lng { get; set; }
        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }
        
        
        [Display(Name = "Max price/room: ")]
        public string MaxPrice { get; set; }
        
        [Display(Name = "Min price/room: ")]
        public string MinPrice { get; set; }
        [Required]
        [Display(Name = "Title: ")]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        [Display(Name = "Text: ")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        [Display(Name = "Link: ")]
        [DataType(DataType.Text)]
        public string Link { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.Accommodation
{
    public class AccommodationVM 
    {
       // public virtual List<BedRoomVM> AvailableRooms { get; set; }
        [Display(Name = "Single Beds ")]
        public int SingleBeds { get; set; }
        [Display(Name = "Double Beds ")]
        public int DoubleBeds { get; set; }
        [Display(Name = "Vacant Single Beds ")]
        public int FreeSingleBeds { get; set; }
        [Display(Name = "Vacant Double Beds ")]
        public int FreeDoubleBeds { get; set; }
        [Display(Name = "Bedrooms ")]
        public int BedRooms { get; set; }
        [Display(Name = "Vacant bedrooms ")]
        public int FreeBedRooms { get; set; }
        public AccommodationVM(Likibu.AccomodationDetails acDetails, string chckIn, string chckOut)
        {

           
            
           
        }
    }
}
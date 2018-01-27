using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models.Accommodation
{
    public class BedRoomVM
    {
        public int ID { get; set; }
        [Display(Name = "Room #")]
        public int RoomNumber { get; set; }
        [Display(Name = "Type")]
        public string RoomType { get; set; }
        [Display(Name = "Double Beds")]
        public int DoubleBeds { get; set; }
        [Display(Name = "Single Beds")]
        public int SingleBeds { get; set; }
        [Display(Name = "Shared")]
        public bool Shared { get; set; }
        public BedRoomVM(BedRoom bedRoom)
        {
            ID = bedRoom.ID;
            RoomNumber = bedRoom.RoomNumber;
            RoomType = bedRoom.roomType.ToString();

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models.Accommodation
{
    public enum RoomType
    {
        None = 0,
        SingleBed =1,
        DoubleBed=2,
        SingleDoubleBed=3,
        MultiBed=4,
        All = 5
    };
  
    
    public class BedRoom:BaseDataObject
    {
        [Display(Name ="RoomType")]
        public RoomType roomType { get; set; }
        [Display(Name = "SingleBeds")]
        public int SingleBeds { get; set; }
        [Display(Name = "DoubleBeds")]
        public int DoubleBeds { get; set; }
        public bool Vacant { get; set; }
        [Display(Name = "Max guests")]
        public int MaxGuests { get; set; }
        [Display(Name = "Room #")]
        public int RoomNumber { get; set; }
        //public int Accommodation_ID { get; set; }
        public bool SharePossible { get; set; }
        
        public BedRoom(int sbed, int dbed, int roomNb)
        {
            
            SharePossible = false;
            SingleBeds = sbed;
            DoubleBeds = dbed;
            MaxGuests = sbed + dbed*2;
            Vacant = false;
            roomType = RoomType.DoubleBed;
            
            RoomNumber = roomNb;
            
        }
        public BedRoom() { }

        public void DetRoomType()
        {
            if (SingleBeds == 1 && DoubleBeds == 0)
            {
                roomType = RoomType.SingleBed;
            }
            if (SingleBeds == 0 && DoubleBeds == 1)
            {
                roomType = RoomType.DoubleBed;
            }
            if (SingleBeds == 1 && DoubleBeds == 1)
            {
                roomType = RoomType.SingleDoubleBed;
                SharePossible = true;
            }
            if (SingleBeds + DoubleBeds > 2 || SingleBeds >= 2)
            {
                roomType = RoomType.MultiBed;
                SharePossible = true;
            }


        }
        
    }

}
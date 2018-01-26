using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.RoomRequest 
{
    /// <summary>
    /// Shows Details of RoomRequest to RequestOwner // used in SelectRooms
    /// </summary>
    public class RoomRequestVMOwner
    {

        public int ID { get; set; }        
        public VM.ProfilePreviewVM ApplyingTraveler { get; set; }
        public string ApplicantName { get; set; }
        [Display(Name ="Kingsizebed:")]
        public bool DoubleBed { get; set; }
        [Display(Name = "Singlebed:")]
        public bool SingleBed { get; set; }
        [Display(Name = "Share:")]
        public bool ShareRoom { get; set; }
        [Display(Name = "CheckIn:")]
        
        public DateTime CheckIn { get; set; }
        [Display(Name = "CheckOut:")]
        
        public DateTime CheckOut { get; set; }
        [Display(Name ="Price:")] //Currency has to be added in View or maybe Controller
        public string Price { get; set; }
        [Display(Name = "Message:")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public int RoomID { get; set; }
        public bool user { get; set; }
        public int Request_ID { get; set; }
        [Display(Name = "RoomType:")]
        public string RoomType { get; set; }
        
        public RoomRequestVMOwner() { }
        public RoomRequestVMOwner(RoomRequest rRq, VM.ProfilePreviewVM applyingTraveler)
        {
            ID = rRq.ID;
            ApplyingTraveler = applyingTraveler;
            ApplicantName = ApplyingTraveler.Name;
            DoubleBed = rRq.DoubleBed>0;
            SingleBed = rRq.SingleBed>0;
            ShareRoom = rRq.ShareRoom;
            CheckIn = rRq.CheckInUser;
            CheckOut = rRq.CheckOutUser;
            Price = rRq.PriceUser;
            RoomID = rRq.Room_ID;
            Request_ID = rRq.Request_ID;
            RoomType = rRq.RoomType.ToString();
        }

        /// <summary>
        /// Used for proposal
        /// </summary>
        /// <param name="rRq"></param>
        public RoomRequestVMOwner(RoomRequest rRq)
        {
            ID = rRq.ID;
            
            DoubleBed = rRq.DoubleBed > 0;
            SingleBed = rRq.SingleBed > 0;
            ShareRoom = rRq.ShareRoom;
            CheckIn = rRq.CheckInUser;
            CheckOut = rRq.CheckOutUser;
            Price = rRq.PriceUser;
            Request_ID = rRq.Request_ID;
           
        }
    }
}
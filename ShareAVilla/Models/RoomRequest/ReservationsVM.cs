using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Models.RoomRequest
{
    public class ReservationsVM
    {
        public int ID { get; set; }       

        public TimeSpan Duration { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<RoomRequestVMOwner> Approvals { get; set; }
        public List<RoomRequestVMOwner> Reservations { get; set; }
        public List<Accommodation.BedRoom> BedRooms { get; set; }
        
        public ReservationsVM(List<Accommodation.BedRoom> Bedrooms, List<RoomRequestVMOwner> reservations, List<RoomRequestVMOwner> approvals, Request.Request rq)
        {
            ID = rq.ID;
            Approvals = approvals;
            Reservations = reservations;
            BedRooms = Bedrooms;
            Duration = rq.CheckOut - rq.CheckIn;
            Start = rq.CheckIn;
            End = rq.CheckOut;
            

        }

        


    }
}
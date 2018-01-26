using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Models.Request
{
    public class SelectRoomVM
    {
        public int ID { get; set; }
        public RoomRequest.RoomRequestVMOwner RoomRequestVMApplicant { get; set; }
        public List<RoomRequest.RoomRequest> ListReservations { get; set; }
        public List<Accommodation.BedRoom> BedRooms { get; set; }

        public SelectRoomVM(RoomRequest.RoomRequestVMOwner rRqVMApplic, List<RoomRequest.RoomRequest> reservations, List<Accommodation.BedRoom> bedrooms)
        {
            RoomRequestVMApplicant = rRqVMApplic;
            ListReservations = reservations;
            BedRooms = bedrooms;
        }
    }
}
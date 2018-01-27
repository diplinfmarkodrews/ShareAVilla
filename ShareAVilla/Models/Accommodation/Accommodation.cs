using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareAVilla.Models.Accommodation
{

    
    public class Capacity:BaseDataObject
    {

     
        public int SingleBed { get; set; }
        public int DoubleBed { get; set; }
        public int NumBedRooms { get; set; }
        public int MaxGuests { get; set; }
        
        public Capacity(int sb, int db, int rooms, int maxG)
        {
            
            SingleBed = sb;
            DoubleBed = db;
            MaxGuests = maxG;
            NumBedRooms = rooms;

        }
        public Capacity() { }
    }
    public class Accommodation : BaseDataObject
    {

        //[ForeignKey("BedRooms")]
        //public int BedRooms_ID { get; set; }
        public List<BedRoom> BedRooms { get; set; }
        [ForeignKey("AccomProfile")]
        public int AccomProfileID { get; set; }
        public AccommodationProfile AccomProfile { get; set; }
        public Capacity Capacity { get; set; }
        public bool Verified { get; set; }
        
        public Accommodation(int maxguests, int numBedRooms)
        {

            if (maxguests == 0) { maxguests = numBedRooms * 2; }
            Verified = GenAbstractionFromAccomProfile(maxguests, numBedRooms);
            
        }
        public Accommodation()
        {
            
            
        }
        private bool GenAbstractionFromAccomProfile(int maxguests, int numBedrooms)
        {
            try
            { 
                int SingleBed = 0;
                int DoubleBed = 0;
                
                int MaxGuests = maxguests;
                int maxGuests = maxguests;
                int NumberBedRooms = numBedrooms;
                List<BedRoom>bedRooms = new List<BedRoom>();
                for (int i = 0; i < NumberBedRooms; i++)
                {
                    BedRoom bdRoom = new BedRoom(0, 1, i + 1);
                    bedRooms.Add(bdRoom);
                    maxGuests -= 2;
                    DoubleBed += 1;
                }
                if (maxGuests > 0) //At least 1 SingleBed + DoubleBed
                {
                    SingleBed = maxGuests;
                    int whileCounter = 0;
                    while (maxGuests > 0)
                    {
                        if (whileCounter > 30) { throw new Exception("WhileCounter Exception at Room Init, Failed to init Accomodation ", new ArgumentException()); }
                        foreach (var room in bedRooms)
                        {
                            if (maxGuests > 0)
                            {
                                room.SingleBeds++;
                                room.MaxGuests++;
                                maxGuests--;
                            }
                        }
                        whileCounter++;
                    }
                }
                else
                {
                    if (maxGuests < 0) //Some rooms have only singlebeds, will only run once
                    {
                        int whileCounter = 0;
                        while (maxGuests < 0)
                        {
                            if (whileCounter > 20) { throw new Exception("WhileCounter Exception at Room Init", new ArgumentException()); }
                            foreach (var room in bedRooms)
                            {
                                if (maxGuests < 0)
                                {
                                    DoubleBed--;
                                    SingleBed++;
                                    room.DoubleBeds--;
                                    room.SingleBeds++;
                                    room.MaxGuests--;
                                    maxGuests++;
                                }
                            }
                            whileCounter++;
                        }
                    }
                }

                if (maxGuests != 0 || DoubleBed < 0 || DoubleBed > MaxGuests / 2 || SingleBed < 0 || SingleBed > MaxGuests) { throw new Exception("Room InitException: MaxGuests " + maxGuests + " DoubleBeds " + DoubleBed + "SingleBeds " + SingleBed, new ArgumentException()); }
                foreach (var room in bedRooms)
                {
                    if (room.DoubleBeds < 0 || room.SingleBeds < 0)
                    {
                        throw new Exception("Room InitException: " + room.DoubleBeds + " DoubleBed " + room.SingleBeds + " SingleBed", new ArgumentException());
                    }
                    else
                    {
                        room.DetRoomType(); // determines Final roomType
                    }
                }

                Capacity = new Capacity(SingleBed, DoubleBed, NumberBedRooms, MaxGuests);
                BedRooms = bedRooms;
                return true;
            }catch(Exception e)
            {
                NotificationManager.AddException(e);
                return false;
            }
           
        }
        public static RoomType DetectRoomType(int sb, int db, bool share, bool nvmnd)
        {

            int DoubleBed = db;
            int SingleBed = sb;

            if (DoubleBed > 0 && SingleBed == 0 && !share)
            {
                return RoomType.DoubleBed;
            }
            if (DoubleBed > 0 && SingleBed > 0 && !share)
            {
                return RoomType.SingleDoubleBed;
            }
            if (DoubleBed == 0 && SingleBed > 0 && !share)
            {
                return RoomType.SingleBed;
            }
            if ((DoubleBed > 0 || SingleBed > 0 ) && share)
            {
                return RoomType.MultiBed;
            }
            if (DoubleBed == 0 && SingleBed == 0 && !nvmnd)
            {
                return RoomType.None;
            }
            else 
            {
                return RoomType.All;
            }

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models
{
    public class TravelerProfileSearchSession:BaseDataObject
    {
        
       
        
        public string Location { get; set; }        
        public string CheckIn { get; set; }        
        public string CheckOut { get; set; }        
        public int BedRooms { get; set; }
        public string LikibuSessionID { get; set; }
        public bool SessionStatus { get; set; }
        public DateTime TimeStamp { get; set; }
        public Models.Find.FilterProperties Filter { get; set; }

        public TravelerProfileSearchSession()
        {
            Filter = new Find.FilterProperties();

            CheckIn = DateTime.Now.ToString("dd.MM.yyyy");
            CheckOut = DateTime.Now.ToString("dd.MM.yyyy");
            TimeStamp = DateTime.Now;
        }
        public void UpdateSearchSession(Find.Search search)
        {
            Location = search.Location;
            CheckIn = search.CheckIn;
            CheckOut = search.CheckOut;
            BedRooms = search.BedRooms;
            LikibuSessionID = search.LikibuSessionID;
            SessionStatus = search.SessionStatus;
            Filter = search.FilterProps;
            TimeStamp = search.TimeStamp;

        }
    }
}
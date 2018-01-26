using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models
{
    public class Review:BaseDataObject
    {       
        public int ProfileReviewsID { get; set; }
        public int TravelerPoster_ID { get; set; }       
        public string Role { get; set; }        
        public string Comment { get; set; }       
        public float Ranking { get; set; }
        public DateTime TimeStamp { get; set; }
        public Review(VM.CreateReviewVM revM, int posterID, int tpRevID)
        {
            TravelerPoster_ID = posterID;
            Role = revM.Role;
            Comment = revM.Comment;
            Ranking = float.Parse((revM.Ranking).Replace('.', ','));
            ProfileReviewsID = tpRevID;
            TimeStamp = DateTime.Now;
        }
        public Review() { }
    }
}
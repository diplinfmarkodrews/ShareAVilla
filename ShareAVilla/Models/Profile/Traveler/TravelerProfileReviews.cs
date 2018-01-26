using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Models
{
    public class TravelerProfileReviews:BaseDataObject
    {        
        public List<Review> Reviews { get; set; }
        public float Score { get; set; }
    }
}
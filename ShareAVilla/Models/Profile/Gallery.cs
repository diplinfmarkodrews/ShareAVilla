using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models
{
    public class Gallery:BaseDataObject
    {
        
        
        public List<Picture> Pics { get; set; }

        
    }
}
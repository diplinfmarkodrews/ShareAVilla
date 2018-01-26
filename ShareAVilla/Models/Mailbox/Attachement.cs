using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models
{
    public class Attachement
    {
        public int ID { get; set; }
        [DataType(DataType.Upload)]
        public string FileName { get; set; }
    }
}
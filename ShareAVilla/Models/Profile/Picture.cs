using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models
{

    public enum PictureType
    {
        Avatar = 1, Photo
    }
    public class Picture:BaseDataObject
    {
        [StringLength(100)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public PictureType PictureType { get; set; }            
        public int Profile_ID { get; set; }
        public byte[] ContentMini { get; set; }
        
    }
}
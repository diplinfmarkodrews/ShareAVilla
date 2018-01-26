using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareAVilla.Models
{
    public class TravelerProfileDescription:BaseDataObject
    {
       
        [Required]
        [Display(Name = "Name: ")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Age: ")]
        public string Age { get; set; }    
        [Display(Name = "Sex: ")]
        [DataType(DataType.Text)]
        public string Sex { get; set; }
        [Display(Name = "Avatar")]
        [ForeignKey("ProfilePic")]
        public int ProfilePic_ID { get; set; }
        public Picture ProfilePic { get; set; }
        [Display(Name = "Text: ")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

    }
}
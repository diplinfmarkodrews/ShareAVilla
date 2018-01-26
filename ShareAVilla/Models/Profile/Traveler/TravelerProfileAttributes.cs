using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models
{
    
    public class TravelerProfileAttributes:BaseDataObject
    {
       
       
        [Display(Name = "Smoker?")]
        public bool IsSmoking { get; set; }
        [Display(Name = "NoiseLevel:")]
        public int NoiseLevel { get; set; }
        [Display(Name = "Brings pets?")]
        public bool HasPets { get; set; }
        [Display(Name = "Vegetarian?")]
        public bool IsVegetarian { get; set; }
        [Display(Name ="Nomad?")]
        public bool Nomad { get; set; }
        [Display(Name = "Yoga")]
        public bool Yoga { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.VM
{

    public class TravelerProfileAttributesVM
    {
        public int ID { get; set; }
        [Display(Name = "Smoker:")]
        public bool IsSmoking { get; set; }
        [Display(Name = "NoiseLevel:")]
        public int NoiseLevel { get; set; }
        [Display(Name = "Brings pets:")]
        public bool HasPets { get; set; }
        [Display(Name = "Vegetarian:")]
        public bool IsVegetarian { get; set; }
        [Display(Name = "Nomad:")]
        public bool Nomad { get; set; }
        [Display(Name = "Yoga:")]
        public bool Yoga { get; set; }
        [Display(Name = "Language:")]
        public string Language { get; set; }
        [Display(Name = "Currency:")]
        public string Currency { get; set; }
        public TravelerProfileAttributesVM() { }
        public TravelerProfileAttributesVM(TravelerProfileAttributes tpA)
        {
            ID = tpA.ID;
            IsSmoking = tpA.IsSmoking;
            NoiseLevel = tpA.NoiseLevel;
            Yoga = tpA.Yoga;
            HasPets = tpA.HasPets;
            IsVegetarian = tpA.IsVegetarian;
            Nomad = tpA.Nomad;
            Currency = tpA.Currency;
            Language = tpA.Language;
        }
    }
}
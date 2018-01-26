using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShareAVilla.Models
{
    public class TravelerProfile:BaseDataObject
    {

        
        
        [Display(Name = "Info: ")]
        [ForeignKey("Description")]
        public int TravelerProfileDescriptionID { get; set; }
        public TravelerProfileDescription Description { get; set; }
        [Display(Name = "Attributes: ")]
        [ForeignKey("Attributes")]
        public int TravelerAttributesID { get; set; }
        public TravelerProfileAttributes Attributes { get; set; }
        [ForeignKey("Mailbox")]
        public int TravelerMailboxID { get; set; }
        public virtual Mailbox Mailbox { get; set; }
        [ForeignKey("Gallery")]
        public int TravelerGalleryID { get; set; }
        public virtual Gallery Gallery { get; set; }

        [ForeignKey("Reviews")]
        public int TravelerReviewsID { get; set; }
        public virtual TravelerProfileReviews Reviews { get; set; }
           

        [ForeignKey("SearchSession")]
        public int TravelerSearchID { get; set; }
        public virtual TravelerProfileSearchSession SearchSession { get; set; }
        public DateTime TimeStamp { get; set; }
        //
        public TravelerProfile() { }
        public void SetValues(Models.VM.TravelerProfileVM tPVM) //only for update
        {
                        
            Description.Age = tPVM.DescriptionVM.Age.ToString("dd.MM.yyyy");
            Description.Name = tPVM.DescriptionVM.Name;
            Description.Sex = tPVM.DescriptionVM.Sex;
            Description.Text = tPVM.DescriptionVM.Text;
            Attributes.Currency = tPVM.AttributesVM.Currency;
            Attributes.Language = "en";     //has to be changed when language is implemented
            Attributes.HasPets = tPVM.AttributesVM.HasPets;
            Attributes.IsSmoking = tPVM.AttributesVM.IsSmoking;
            Attributes.IsVegetarian = tPVM.AttributesVM.IsVegetarian;
            Attributes.NoiseLevel = tPVM.AttributesVM.NoiseLevel;
            Attributes.Nomad = tPVM.AttributesVM.Nomad;
            Attributes.Yoga = tPVM.AttributesVM.Yoga;           
        }
    }
}
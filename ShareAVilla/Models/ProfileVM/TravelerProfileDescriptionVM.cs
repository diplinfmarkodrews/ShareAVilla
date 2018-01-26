using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShareAVilla.Models.VM
{
    public class TravelerProfileDescriptionVM
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Name: ")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Age: ")]
        public DateTime Age { get; set; }    
        [Display(Name = "Sex: ")]
        [DataType(DataType.Text)]
        public string Sex { get; set; }
        [ForeignKey("ProfilePic")]
        public int ProfilePic_ID { get; set; }
        public Picture ProfilePic { get; set; }
        [Display(Name = "Avatar")]
        public HttpPostedFileBase upload { get; set; }
        [Display(Name = "Text: ")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public TravelerProfileDescriptionVM()
        {
           
        }
        public TravelerProfileDescriptionVM(TravelerProfileDescription tpD)
        {
            ID = tpD.ID;
            Name = tpD.Name;
            Age = DateTime.Parse(tpD.Age);
            //DateTime age = DateTime.Parse(Age);
            //Age = age.ToString("dd.MM.yyyy");
            Sex = tpD.Sex;
            Text = tpD.Text;
            ProfilePic = tpD.ProfilePic;
        }
    }
    
}
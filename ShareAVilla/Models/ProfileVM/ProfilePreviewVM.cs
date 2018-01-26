using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models.VM
{ 
    public class ProfilePreviewVM
    {
        public int ID { get; set; }
        public Picture ProfilePic { get; set; }
        public string Name { get; set; }
        public float Score { get; set; }
        //Todo load reviews
        public ProfilePreviewVM(TravelerProfile profil)
        {
            ID = profil.ID;
            ProfilePic = profil.Description.ProfilePic;
            Name = profil.Description.Name;
            Score = profil.Reviews.Score;
        }
    }
}
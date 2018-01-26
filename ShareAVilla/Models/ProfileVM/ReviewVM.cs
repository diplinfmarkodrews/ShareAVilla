using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla.Models.VM
{
    public class ReviewVM
    {

        public int ID { get; set; }
        [Display(Name = "Users Role: ")]
        public string Role { get; set; }        
        [Display(Name = "Score:")]
        public float Points { get; set; }
        [Display(Name = "Comment:")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public ProfilePreviewVM Poster { get; set; }
        public ReviewVM(Review rev)
        {
            ID = rev.ID;
            Role = rev.Role;
            Points = rev.Ranking;
            Comment = rev.Comment;            
        }
    }
}
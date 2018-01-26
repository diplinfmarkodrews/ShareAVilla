using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShareAVilla.Models.VM
{
    public class CreateReviewVM
    {
        public int ID { get; set; }
        [Display(Name ="His/her role:")]
        public string Role { get; set; }
        [Display(Name = "Score:")]
        public string Ranking { get; set; }
        [Display(Name ="Your Comment:")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public CreateReviewVM(int id)
        {
            ID = id;
        }
        public CreateReviewVM() { }
    }
}
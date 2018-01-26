using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShareAVilla.Models.VM
{
    public class TravelerProfileVM
    {

        public int ID { get; set; }
        
        [Display(Name = "Info: ")]
        [ForeignKey("DescriptionVM")]
        public int TravelerProfileDescriptionID { get; set; }
        public TravelerProfileDescriptionVM DescriptionVM { get; set; }
        [Display(Name = "Attributes: ")]
        [ForeignKey("AttributesVM")]
        public int TravelerAttributesID { get; set; }
        public TravelerProfileAttributesVM AttributesVM { get; set; }
        public bool User { get; set; }

        [ForeignKey("Reviews")]
        public int TravelerReviewsID { get; set; }
        public TravelerReviewsVM ReviewsVM { get; set; }
        public CreateReviewVM createReview { get; set; }
        //SearchSession   

        public TravelerProfileVM()
        {
            AttributesVM = new TravelerProfileAttributesVM();
            DescriptionVM = new TravelerProfileDescriptionVM();
        }
        public TravelerProfileVM(TravelerProfile tp)
        {
            ID = tp.ID;
            DescriptionVM = new TravelerProfileDescriptionVM(tp.Description);
            ReviewsVM = new TravelerReviewsVM(tp.Reviews);
            AttributesVM = new TravelerProfileAttributesVM(tp.Attributes);
            
        }
    
      
    }
}
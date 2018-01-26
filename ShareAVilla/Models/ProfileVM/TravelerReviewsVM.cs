using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Models.VM
{
    public class TravelerReviewsVM
    {
        public int ID { get; set; }
        public List<ReviewVM> Reviews { get; set; }
        public float Score { get; set; }
        public TravelerReviewsVM(TravelerProfileReviews reviews)
        {
            ID = reviews.ID;
            Reviews = new List<ReviewVM>();
            Score = 0.0f;
            if (reviews.Reviews != null)//kann raus!
            {
                foreach (var rev in reviews.Reviews)
                {
                    ReviewVM revM = new ReviewVM(rev);
                    Score = Score + rev.Ranking;
                    Reviews.Add(revM);
                }
                if (reviews.Reviews.Count > 0)
                {
                    Score = Score / Reviews.Count;
                }
                
            }
        }
    }
}
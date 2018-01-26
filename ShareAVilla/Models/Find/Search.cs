using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareAVilla.Models.Find
{

    public class Search:BaseDataObject
    {
        
        [Display(Name = "Where: ")]
        [Required]
        [DataType(DataType.Text)]
        public string Location { get; set; }
        [Required]
        [Display(Name = "CheckIn: ")]
        [DataType(DataType.Date)]
        public string CheckIn { get; set; }
        [Required]
        [Display(Name = "CheckOut: ")]
        [DataType(DataType.Date)]
        public string CheckOut { get; set; }
        [Display(Name = "Bedrooms: ")]
        public int BedRooms { get; set; }

        public string nav { get; set; }
        [Display(Name = "Filter: ")]
        public FilterProperties FilterProps { get; set; }        
        public bool SessionStatus { get; set; }
        public DateTime TimeStamp { get; set; }
        public string LikibuSessionID { get; set; }
    }
    public class FilterProperties : BaseDataObject
    {

        [Display(Name = "Results per page: ")]
        public string PerPage { get; set; }
        [Display(Name = "Page: ")]
        public string Page { get; set; }
        [Display(Name = "Sort: ")]
        public string Sort { get; set; }
        [Display(Name = "Privacy: ")]
        public string PrivacyType { get; set; }
        [Display(Name = "Type: ")]
        public string AcomType { get; set; }
        [Display(Name = "Amenities: ")]
        public string Amenities { get; set; }
        public string PartnerID { get; set; }
        [Display(Name = "Min Price: ")]
        public string PriceMin { get; set; }
        [Display(Name = "Max Price: ")]
        public string PriceMax { get; set; }
        [Display(Name = "kitchen")]
        public bool kitchen { get; set; }
        ///Todo amenities integration
        public FilterProperties()
        {
            PerPage = "25";
            Page = "1";
            Sort = "";// Models.Find.Likibu.Sort.Reco;
            AcomType = "";
            PartnerID = "";
            PrivacyType = "";
            Amenities = "";
            PriceMin = "";
            PriceMax = "";

        }
    }
}
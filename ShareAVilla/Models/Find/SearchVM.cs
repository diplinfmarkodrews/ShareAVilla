using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Models.Find
{
    public class Marker
    {
        public int ID { get; set; }
        public string Title{ get; set; }
        public string Type { get; set; }
        public string Bedrooms { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string Price { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public override string ToString()
        {
            string res = "title: " + Title + "\ntype: " + Type + "\nbedrooms: " + Bedrooms + "\nprice: " + Price + "\nLat: " + Lat + "\nLng: " + Lng;
            return res;
        }
    }

    public class RequestResults:BaseDataObject
    {
        public List<Request.RequestVMListing> RequestList { get; set; }
        public int TotalResults { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public RequestResults()
        {
            RequestList = new List<Request.RequestVMListing>();
            TotalResults = 0;
            Page = 1;
            PerPage = 10;
        }
        public RequestResults(List<Request.RequestVMListing> res, int page, int perpage, int totalres)//, int page, int perpage)
        {
            
            TotalResults = totalres;
            RequestList = res;
            Page = page;
            PerPage = perpage;
            
        }
    }

    public class SearchVM:BaseDataObject
    {
       
        public Search Search { get; set; }        
        public string Language { get; set; }
        public string Currency { get; set; }
        public RequestResults RequestResults { get; set; }
        public Accommodation.Likibu.SearchRequestResults LikibuResults { get; set; }
        public SearchVM()
        {
            Search Search = new Search();
            RequestResults = new RequestResults(); 

            LikibuResults = new Accommodation.Likibu.SearchRequestResults();           
        }
        public SearchVM(Search search, string lang, string cur)
        {
            Language = lang;
            Currency = cur;
            Search Search = search;
            LikibuResults = new Accommodation.Likibu.SearchRequestResults();
            RequestResults = new RequestResults();
            
        }
    }
    
}
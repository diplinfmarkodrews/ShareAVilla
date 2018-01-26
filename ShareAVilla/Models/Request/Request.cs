using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareAVilla.Models.Request
{
    public enum RequestStatus
    {
        Valid=1,
        Closed=2,
        Deleted
    }

    public enum RequestType
    {
        Free,
        Bound
    }
    /// <summary>
    /// 
    /// </summary>
    /// 

    public class Request : BaseDataObject
    {



        [ForeignKey("Accommodation")]
        public int Accommodation_ID { get; set; }
        public Accommodation.Accommodation Accommodation { get; set; }
        public RequestType Type { get; set; }
        public int RequestOwner_ID { get; set; }
        public string PriceMax { get; set; }
        public string PriceMin { get; set; }
        public DateTime CheckIn { get; set; }        //RqOwner can change Dates
        public DateTime CheckOut { get; set; }
        public string SalesPricePWeek { get; set; }   ///Rqowner can define own Prices 
        public string SalesPricePDay { get; set; }
        public string SalesPricePMonth { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }          
        public DateTime StartTimeStamp { get; set; }
        public Request()
        {
          
        }
        /// <summary>
        /// Create Request -> transform RequestVM to Request
        /// </summary>
        /// <param name="acDetails"></param>
        /// <param name="RqOwnerID"></param>
        /// <param name="requestVMOwner"></param>
        public Request(int RqOwnerID, RequestVMOwner requestVMOwner)
        {

        
            RequestOwner_ID = RqOwnerID;
            
            CheckIn = requestVMOwner.CheckIn;
            CheckOut = requestVMOwner.CheckOut;
            Type = RequestType.Bound;
           
            Text = requestVMOwner.Text;
            Title = requestVMOwner.Title;


            SalesPricePDay = requestVMOwner.SalesPricePNight;
            SalesPricePWeek = requestVMOwner.SalesPricePWeek;
            SalesPricePMonth = requestVMOwner.SalesPricepMonth;

            StartTimeStamp = DateTime.Now;
        }
        public static string CurrencySign(string cur)
        {
            switch (cur)
            {
                case "usd": return "$";
                case "eur": return "€";
                default: return "$";
            }

        }
        public static string GetCurrency(string price)
        {
            if (price.Contains("$"))
            {
                return "$";
            }
            if (price.Contains("€"))
            {
                return "€";
            }
            else { return ""; }
        }
        /// <summary>
        /// Puts a currency to price string if exists
        /// erases any other currency then eur and usd
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string PriceCurrency(string price)
        {
            if (price.Contains("$ €"))
            {
                return price;
            }
            
            return price + "$";
        }
        public static string PriceCurrency(string price, string cur)
        {
            if (price.Contains("$")|| price.Contains("€"))
            {
                return price;
            }

            return price + cur;
        }
        
        /// <summary>
        /// returns the price as float w/o currency
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        ///         
        public static float Price(string price)
        {
            char[] trim = { '€', '$' };
            price = price.TrimEnd(trim);
            return float.Parse(price);
        }
        public Request(int RqOwnerID, RequestVMFree requestVM)
        {
            RequestOwner_ID = RqOwnerID;
            CheckIn = requestVM.CheckIn;
            CheckOut = requestVM.CheckOut;
            Type = RequestType.Free;
            Text = requestVM.Text;
            Title = requestVM.Title;
            PriceMax = PriceCurrency(requestVM.MaxPrice);
            PriceMin = PriceCurrency(requestVM.MinPrice);
            StartTimeStamp = DateTime.Now;

        }
        /// <summary>
        /// Update Request
        /// </summary>
        /// <param name="requestVM"></param>        
        //needed for Edit
        public void SetValues(RequestVMOwner requestVM )
        {
            
            CheckIn = requestVM.CheckIn;
            CheckOut = requestVM.CheckOut;
            Text = requestVM.Text;
            Title = requestVM.Title;
            if (requestVM.requestType == RequestType.Bound)
            {
                SalesPricePDay = (requestVM.SalesPricePNight);
                SalesPricePWeek = (requestVM.SalesPricePWeek);
                SalesPricePMonth = (requestVM.SalesPricepMonth);
            }
            if (requestVM.requestType == RequestType.Free)
            {
                PriceMax = (requestVM.MaxPrice);
                PriceMin = (requestVM.MinPrice);

                Accommodation.AccomProfile.URL = requestVM.Link;
                Accommodation.AccomProfile.Lat = requestVM.Lat;
                Accommodation.AccomProfile.Lng = requestVM.Lng;
            }
        }
        public static float CalcPrice(DateTime start, DateTime end, float pricePDay, float pricePWeek, float pricePMonth, int BedRooms)
        {
            TimeSpan diff = end - start;
            int days = diff.Days; 
            if(days>=30)
            {
                return (pricePMonth / 31) * days / BedRooms;
            }
            if (days >= 7)
            {
                return (pricePWeek / 7) * days / BedRooms;
            }
            else
            {
                return pricePDay * days / BedRooms;
            }
            
        }
      

    }
    
}
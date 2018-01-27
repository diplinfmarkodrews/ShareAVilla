using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Google.Apis.Services;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ShareAVilla.Controllers
{
    public static class GoogleWrapper
    {
        private const string GoogleApiKey = "&key=AIzaSyDANY6GhGSZlKBaGHAu0Grw4gh0cQgSjGU";
        private const string GoogleRequest = "https://maps.googleapis.com/maps/api/geocode/json?address=";

        
        private static string GoogleGeoRequestUri(string adress)
        {
            return GoogleRequest + adress + GoogleApiKey;
        }
        public static float[] LongLat2DoubleAr(string LongLat)
        {
            string[] llstring = LongLat.Split('/');
            float[] llfloat = new float[2];
            llfloat[0] = float.Parse(llstring[0]);
            llfloat[1] = float.Parse(llstring[1]);
            return llfloat;
        }
        public static async Task<string> GetGeolocation(string adress)
        {

            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            string ret = await webClient.DownloadStringTaskAsync(GoogleGeoRequestUri(adress));
            return ParseJSON4LongLat(ret);

        }
       
        private static string ParseJSON4Status(string js)
        {
            var jo = JObject.Parse(js);
            return jo["status"].ToString();            
        }
        private static string ParseJSON4LongLat(string js)
        {
  
            string status = ParseJSON4Status(js);
            
            if (string.Compare(status, "OK") == 0)
            {

                var jo = JObject.Parse(js);
                string lat = jo["results"][0]["geometry"]["location"]["lat"].ToString();
                string lng = jo["results"][0]["geometry"]["location"]["lng"].ToString();
                return lng + "/" + lat;
            }
            else {
                NotificationManager.AddException(new Exception("GoogleGeolocationException status:"+status));
                return "";
            }
        }
   

    }
}
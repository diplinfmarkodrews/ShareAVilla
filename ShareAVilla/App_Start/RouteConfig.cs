using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShareAVilla
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "SelectFlatmatesUpdate",
               url: "RoomRequests/SelectFlatmates/{id}/{update}/{rrqid}",
               defaults: new { controller = "RoomRequests", action = "SelectFlatmates", id = "", update = "", rrqid = "" }
           );

          
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

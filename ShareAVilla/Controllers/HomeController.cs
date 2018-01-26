using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace ShareAVilla.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                Models.TravelerProfile tp = await ProfileManager.LoadUserAndTravelerProfile(User);
                if (tp == null)
                {
                    return RedirectToAction("Create", "Profiles");
                } 
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(Models.Find.SearchVM searchVM) //public search
        {
            searchVM.Currency = "usd";
            searchVM.Language = "en";
           

          
            return View(searchVM);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Share-A-Villa: The Accommodation Plattform for Expats, Nomads and Remoteworkers";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "If you have any issues, contact me: ShareAVilla@gmx.net";

            return View();
        }
    }
}
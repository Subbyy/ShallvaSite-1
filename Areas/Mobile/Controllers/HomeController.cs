using ShallvaMVC.Models;
using ShallvaMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShallvaMVC.Areas.Mobile.Controllers
{
    public class HomeController : Controller
    {
        // GET: Mobile/Home
        public ActionResult Index()
        {
            ViewData["Banners"] = DataProvider.GetBannersGallery();
            return View();
        }
        public ActionResult About()
        {
            string aboutText = DataProvider.GetAboutContent();
            return View((object)aboutText);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact model)
        {
            DataProvider.SendContact(model);
            ViewData["ContactSent"] = true;
            return View();
        }
    }
}
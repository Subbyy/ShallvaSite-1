using ShallvaMVC.Models;
using ShallvaMVC.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ShallvaMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           
            if (User.Identity.IsAuthenticated)
            {
                SiteUser currentUser = DataProvider.GetUsers(User.Identity.GetUserId()).Single();
                ShallvaUser.SetCurrentUser(currentUser.RecordId, currentUser.Name);

                if(Session["Cart"] == null)
                {
                    var cart = DataProvider.GetCart(currentUser.RecordId);

                    if (cart.Items.Count > 0)
                    {
                        ViewData["HasCart"] = true;
                    }

                    Session["Cart"] = cart;
                }
            }

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
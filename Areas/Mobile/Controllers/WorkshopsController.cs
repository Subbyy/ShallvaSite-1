using ShallvaMVC.Models;
using ShallvaMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShallvaMVC.Areas.Mobile.Controllers
{
    public class WorkshopsController : Controller
    {
        public ActionResult Workshops()
        {
            return View(new Workshops());
        }

        [HttpPost]
        public void Contact(ContactModal model)
        {
            DataProvider.SendContact(model);
        }
    }
}
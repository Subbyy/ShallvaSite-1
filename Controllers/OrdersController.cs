using ShallvaMVC.Models;
using ShallvaMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShallvaMVC.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        public ActionResult Index()
        {
            if (Session["OrderRemoved"] != null)
            {
                int orderId = (int)Session["OrderRemoved"];
                Session["OrderRemoved"] = null;
                ViewData["Message"] = "הזמנה מספר " + orderId + " נמחקה בהצלחה";
            }
            else if(Session["OrderApproved"] != null)
            {
                int orderId = (int)Session["OrderApproved"];
                Session["OrderApproved"] = null;
                ViewData["Message"] = "הזמנה מספר " + orderId + " אושרה בהצלחה";
            }

            List<OrderModel> orders = DataProvider.GetOrders(ShallvaUser.Current.Id);

            return View(orders);
        }

        [HttpPost]
        public ActionResult RemoveOrder(int orderId)
        {
            DataProvider.RemoveOrder(orderId);
            Session["OrderRemoved"] = orderId;
            return RedirectToAction("Index", "Orders");
        }

        [HttpPost]
        public ActionResult ApproveOrder(int orderId, string UserMessage, string UserName)
        {
            DataProvider.ApproveOrder(orderId, UserMessage, UserName);
            Session["OrderApproved"] = orderId;
            return RedirectToAction("Index", "Orders");
        }
    }
}


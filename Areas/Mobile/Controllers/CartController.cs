using ShallvaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ShallvaMVC.Utils;

namespace ShallvaMVC.Areas.Mobile.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            return View(DataProvider.GetCart(ShallvaUser.Current.Id));
        }

        [HttpPost]
        public JsonResult AddProductsToCart(Product p)
        {
            DataProvider.AddProductToCart(p, ShallvaUser.Current.Id);

            return Json(new { });
        }

        [HttpPost]
        public JsonResult UpdateCartProduct(int orderId, int quantity, bool isChecked, string comment)
        {
            CartProduct cp = new CartProduct()
            {
                Comment = comment,
                IsChecked = isChecked,
                Quantity = quantity,
                OrderId = orderId
            };

            DataProvider.UpdateCartProduct(cp);

            return Json(new { });
        }

        [HttpPost]
        public JsonResult RemoveProductFromCart(int orderId)
        {
            DataProvider.RemoveProductFromCart(orderId);

            return Json(new { });
        }
    }
}
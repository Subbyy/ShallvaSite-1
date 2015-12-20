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
using System.IO;

namespace ShallvaMVC.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index(int orderId = 0)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(DataProvider.GetCart(ShallvaUser.Current.Id, orderId));
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

        [HttpPost]
        public ActionResult ConfirmOrder(string UserName, string UserMessage)
        {
            Cart cart = DataProvider.GetCart(ShallvaUser.Current.Id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Print(int orderId = 0)
        {
            Cart cart = DataProvider.GetCart(ShallvaUser.Current.Id, orderId);
            //return View(cart);
            string html = this.RenderPartialToString("Print", cart);
            
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            
            var res = htmlToPdf.GeneratePdf(html);

            //MemoryStream ms = new MemoryStream();
            //var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
            //pdf.Save(ms);
            //Byte[] res = ms.ToArray();
            ////HttpContext.Response.AddHeader("content-disposition", "attachment; order-" + cart.OrderId + ".pdf");
            return File(res, "application/pdf", "order-" + orderId + ".pdf");
        }
    }
}
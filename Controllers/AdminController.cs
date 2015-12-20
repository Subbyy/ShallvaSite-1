using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using ShallvaMVC.Models;
using ShallvaMVC.Filters;
using ShallvaMVC.Utils;
using System.IO;
using System.Threading.Tasks;

namespace ShallvaMVC.Controllers
{
    [AdminOnlyFilter]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            string aboutText = DataProvider.GetAboutContent();
            return View((object)aboutText);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult About(string aboutText)
        {
            DataProvider.UpdateAboutContent(aboutText);
            return View((object)aboutText);
        }

        public ActionResult ManageMainCategories()
        {
            return View(GetCategories(true));
        }

        public ActionResult ManageSubCategories(int mainCat = 0)
        {
            if (mainCat == 0)
                return RedirectToAction("ManageMainCategories");

            List<CategoryListItem> categories = GetCategories();

            CategoryListItem mainCategory = categories.First(x => x.Id == mainCat);

            Session["MainCategory"] = mainCat;
            Session["MainCategoryName"] = mainCategory.Name;

            return View(mainCategory.SubCategories);
        }

        [HttpPost]
        public JsonResult AddMainCategory(string categoryName)
        {
            int orderId;
            int catId = DataProvider.AddMainCategory(categoryName, out orderId);

            CategoryListItem model = new CategoryListItem()
            {
                Id = catId,
                Name = categoryName,
                OrderId = orderId
            };

            if (Session["Categories"] != null)
            {
                List<CategoryListItem> categories = (List<CategoryListItem>)Session["Categories"];
                categories.Add(model);
                Session["Categories"] = categories;
            }

            string html = this.RenderPartialToString("Partials/_MainCategoryRow", model);

            return Json(new { html = html });
        }

        [HttpPost]
        public JsonResult AddSubCategory(int mainCat, string subCat)
        {
            string mainCatName = GetCategories().First(x => x.Id == mainCat).Name;

            int orderId;
            int subCatId = DataProvider.AddSubCategory(subCat, mainCatName, mainCat, out orderId);

            CategoryListItem model = new CategoryListItem()
            {
                Id = subCatId,
                Name = subCat,
                OrderId = orderId
            };

            ViewData["MainCategory"] = mainCat;
            string html = this.RenderPartialToString("Partials/_SubCategoryRow", model);

            return Json(new { html = html });
        }

        public ActionResult HomePictures()
        {
            ViewData["FileName"] = DateTime.Now.Ticks.ToString();
            return View(DataProvider.GetBannersGallery());
        }

        [HttpPost]
        public JsonResult DummayUpload()
        {
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult AddBanner(string title, string subTitle, string filename)
        {
            int orderId;
            int id = DataProvider.InsertHomeBanner(title, subTitle, filename, out orderId);
            BennerGalleryItem banner = new BennerGalleryItem()
            {
                Id = id,
                OrderId = orderId,
                ImageName = filename,
                Title = title,
                SubTitle = subTitle
            };

            string html = this.RenderPartialToString("Partials/_HomeBannerRow", banner);

            return Json(new { success = true, html = html });
        }

        [HttpPost]
        public JsonResult RemoveBanner(int bannerId)
        {
            DataProvider.RemoveHomeBanner(bannerId);
            return Json(new { success = true });
        }

        public JsonResult UploadAttachment(bool? isSmallPic = null)
        {
            var uploadedFiled = Request.Files[0];
            string ext = uploadedFiled.FileName.Split('.')[1].ToLower();
            string filename = string.Empty;
            bool isSuccess = true;

            string[] extentions = { "bmp", "gif", "ico", "jpg", "jpeg", "png", "tif", "tiff", "wmf" };

            if (extentions.Contains(ext))
            {
                filename = DateTime.Now.Ticks + "." + ext;

                string fullPath = string.Empty;

                if (Request.Files.AllKeys.Contains("sp"))
                {
                    fullPath = "small_images\\";
                }

                string path = Server.MapPath("/") + "Content\\Images\\" + fullPath + filename;
                uploadedFiled.SaveAs(path);
            }
            else
            {
                isSuccess = false;
            }

            return Json(new { success = isSuccess, filename = filename });
        }

        public ActionResult ManageProducts(int? category, string search = "", string catName = "")
        {
            if (Session["MainCategory"] == null || Session["MainCategoryName"] == null)
            {
                return RedirectToAction("ManageMainCategories", "Admin");
            }

            List<PictureProduct> products = DataProvider.GetPictureProducts(category, search);

            if (category.HasValue)
                Session["SubCat"] = category.Value;
            else
                Session["SubCat"] = null;

            Session["SubCatName"] = catName;

            return View(products);
        }

        [HttpPost]
        public JsonResult AddPictureProduct(string title, string bigImg, string smallImg, int subCatId, string subCatName, int mainCatId, string mainCatName)
        {
            int orderId;
            int id = DataProvider.AddProductPicture(title, smallImg, bigImg, subCatId, subCatName, mainCatId, mainCatName, out orderId);

            PictureProduct p = new PictureProduct()
            {
                Id = id,
                ImageName = bigImg,
                IsActive = true,
                MainCategoryId = mainCatId,
                MainCategoryName = mainCatName,
                OrderId = orderId,
                SmallImageName = smallImg,
                SubCategoryId = subCatId,
                SubCategoryName = subCatName,
                Title = title
            };

            string html = this.RenderPartialToString("Partials/_ProductPicRow", p);

            return Json(new { success = true, html = html });
        }

        public ActionResult ManageProductTables(string title, int pic, string search = null)
        {
            if (Session["MainCategory"] == null || Session["MainCategoryName"] == null || Session["SubCatName"] == null)
            {
                return RedirectToAction("ManageMainCategories", "Admin");
            }


            List<ProductTable> tables = DataProvider.GetProductTables(pic, search);
            Session["PicId"] = pic;
            Session["PicTitle"] = title;
            return View(tables);
        }

        public ActionResult ManageProductTableRows(int ptid, string ptname, string search = null)
        {
            if (Session["MainCategory"] == null || Session["MainCategoryName"] == null || Session["SubCatName"] == null || Session["PicId"] == null || Session["PicTitle"] == null)
            {
                return RedirectToAction("ManageMainCategories", "Admin");
            }

            List<SubProductRow> subProds = DataProvider.GetSubProducts(ptid, search);

            Session["SubProdId"] = ptid;
            Session["SubProdName"] = ptname;

            return View(subProds);
        }

        [HttpPost]
        public JsonResult AddProductTableRow(string sbName, string sku)
        {
            int picId = (int)Session["PicId"];
            int ptid = (int)Session["SubProdId"];
            int subCatId = (int)Session["SubCat"];
            int mainCatId = (int)Session["MainCategory"];

            int id = DataProvider.AddSubProduct(sbName, sku, picId, ptid, subCatId, mainCatId);

            SubProductRow sbr = new SubProductRow()
            {
                Id = id,
                MainCategoryId = mainCatId,
                Description = "",
                MainCategoryName = Session["MainCategoryName"].ToString(),
                OrderId = 0,
                SKU = sku,
                ProductPictureId = picId,
                ProductPictureTitle = Session["PicTitle"].ToString(),
                ProductTableId = ptid,
                SubCategoryId = subCatId,
                SubCategoryName = Session["SubCatName"].ToString(),
                SubProductName = Session["SubProdName"].ToString()
            };

            string html = this.RenderPartialToString("Partials/_SubProductRow", sbr);

            return Json(new { html = html });
        }

        [HttpPost]
        public JsonResult GetSubProductProperties(int subCatId)
        {
            List<SubProductProperty> model = DataProvider.GetSubProductProperties(subCatId);
            string html = this.RenderPartialToString("Partials/_SubProductProps", model);
            return Json(new { html = html });
        }

        [HttpPost]
        public JsonResult AddSubProductProperty(string propName, int subCatId)
        {
            int propId = DataProvider.AddSubProductProperty(subCatId, propName);
            SubProductProperty prop = new SubProductProperty()
            {
                Id = propId,
                Name = propName
            };
            string html = this.RenderPartialToString("Partials/_SubProductPropRow", prop);

            return Json(new { html = html });
        }

        [HttpPost]
        public JsonResult AddSubProductTable(string tableName, int picId, int? subCatId)
        {
            int id = DataProvider.AddSubProductTable(subCatId, picId, tableName);
            ProductTable table = new ProductTable()
            {
                Id = id,
                PictureId = picId,
                Title = tableName,
                PictureName = Session["PicTitle"].ToString()
            };

            string html = this.RenderPartialToString("Partials/_ProductTableRow", table);
            return Json(new { html = html });
        }

        public ActionResult ManageUsers()
        {
            List<SiteUser> users = DataProvider.GetUsers();
            return View(users);
        }

        [HttpPost]
        public JsonResult AddUser(SiteUser model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, LoginName = model.Name, PhoneNumber = model.Phone };

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var result = userManager.Create(user, model.Password);
            model.Id = user.Id;

            string html = this.RenderPartialToString("Partials/_UserRow", model);

            return Json(new { success = result.Succeeded, userId = user.Id, html = html });
        }

        [HttpPost]
        public JsonResult UpdateUser(SiteUser model)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = userManager.FindById(model.Id);

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.LoginName = model.Name;

            var result = userManager.Update(user);

            return Json(new { success = result.Succeeded });
        }

        [HttpPost]
        public JsonResult RemoveUser(string userId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            var result = userManager.Delete(user);

            return Json(new { success = result.Succeeded });
        }

        [HttpPost]
        public JsonResult ChangeUserPassword(string userId, string password)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            userManager.RemovePassword(userId);
            userManager.AddPassword(userId, password);

            return Json(new { success = true });
        }


        private List<CategoryListItem> GetCategories(bool getAll = false)
        {
            List<CategoryListItem> categories = null;
            if (Session["Categories"] == null)
            {
                categories = DataProvider.GetCategories(getAll);
                Session["Categories"] = categories;
            }
            else
            {
                categories = (List<CategoryListItem>)Session["Categories"];
            }

            return categories;
        }
    }
}
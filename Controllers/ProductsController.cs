
using ShallvaMVC.Models;
using ShallvaMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShallvaMVC.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult ProductsList(int? categoryId = null, int? subCategoryId = null, int page = 1)
        {
            List<Product> products = GetProductsByCriteria(categoryId, subCategoryId, page);
            return View(products);
        }

        [HttpPost]
        public JsonResult GetNextProducts(int? categoryId = null, int? subCategoryId = null, int page = 1)
        {
            List<Product> products = GetProductsByCriteria(categoryId, subCategoryId, page);

            return Json(new { html = this.RenderPartialToString("Partials/_ProdcutsItems", products), lastPage = products.Count < 20 });
        }

        private List<Product> GetProductsByCriteria(int? categoryId = null, int? subCategoryId = null, int page = 1, int? productId = null)
        {
            if (page <= 0)
                page = 1;

            CategoryListItem nextCategory;

            ProductsCriteria criteria = new ProductsCriteria()
            {
                PageSize = 20,
                PageNumber = page,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                ProductId = productId
            };


            List<Product> products = DataProvider.GetProductsByCriteria(criteria, out nextCategory);

            if (nextCategory != null)
            {
                ViewData["NextCategory"] = nextCategory;
            }

            ViewData["CategoriesTags"] = DataProvider.GetCategoriesTags();
            ViewData["Category"] = categoryId != null ? categoryId.Value : -1;
            ViewData["SubCategory"] = subCategoryId != null ? subCategoryId.Value : -1;
            ViewData["PageNumber"] = page;

            return products;
        }

        public ActionResult Product(int productId)
        {
            List<Product> similarProducts;
            var product = DataProvider.GetProduct(productId, out similarProducts);
            ViewData["SimilarProducts"] = similarProducts;
            return View(product);
        }

    }
}

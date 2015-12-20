using ShallvaMVC.Areas.Mobile.Models;
using ShallvaMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace ShallvaMVC.Utils
{
    /// <summary>
    /// TODO: insert to cache if needed and check before call to database
    /// </summary>
    public class DataProvider
    {
        private DataProvider() { }
        private const string CONNECTION_STRING = "DefaultConnection";

        public static ShallvaUser GetCurrentUser(string userName)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetUserIdByUserName", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserName", userName));
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            ShallvaUser.SetCurrentUser(id, name);
                        }
                    }

                    conn.Close();
                }
            }

            return ShallvaUser.Current;
        }

        public static void SendContact(Contact contact)
        {
            var fromAddress = ConfigurationManager.AppSettings["EmailFrom"];
            var toAddress = ConfigurationManager.AppSettings["EmailTo"];
            string fromPassword = ConfigurationManager.AppSettings["EmailFromPassword"];

            string subject = contact.Subject;
            string body = "מאת: " + contact.Name + "\n";
            body += "אימייל: " + contact.Email + "\n";
            body += "טלפון: " + contact.Phone + "\n";
            body += "טלפון נייד: " + contact.Mobile + "\n";
            body += "שם העסק: " + contact.BusinessName + "\n";
            body += "ת.ז / ע.מ / ח.פ: " + contact.Id + "\n";
            body += "כתובת העסק: " + contact.BusinessAddress + "\n";
            body += "נושא: " + contact.Subject + "\n";
            body += "הודעה: \n" + contact.Content + "\n";

            body = HttpUtility.HtmlEncode(body);

            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 20000;
            }
            smtp.Send(fromAddress, toAddress, subject, body);
        }

        public static void SendContact(ContactModal contact)
        {
            var fromAddress = ConfigurationManager.AppSettings["EmailFrom"];
            var toAddress = ConfigurationManager.AppSettings["ModalEmailTo"];
            string fromPassword = ConfigurationManager.AppSettings["EmailFromPassword"];

            string subject = "קבלת עדכונים על פעילויות חדשות";
            string body = "מאת: " + contact.Name + "\n";
            body += "אימייל: " + contact.Email + "\n";
            body += "טלפון: " + contact.Phone + "\n";

            body = HttpUtility.HtmlEncode(body);

            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 20000;
            }

            smtp.Send(fromAddress, toAddress, subject, body);


        }

        public static List<BennerGalleryItem> GetBannersGallery()
        {
            List<BennerGalleryItem> banners = new List<BennerGalleryItem>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBannersGallery", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BennerGalleryItem banner = new BennerGalleryItem()
                            {
                                Id = reader.GetInt32(0),
                                OrderId = reader.GetInt32(4),
                                Title = reader.GetString(1),
                                SubTitle = reader.GetString(2),
                                ImageName = reader.GetString(3)
                            };

                            banners.Add(banner);
                        }
                    }

                    conn.Close();
                }
            }

            return banners;
        }

        public static List<CategoryListItem> GetCategories(bool getAll = false)
        {
            List<CategoryListItem> categories = new List<CategoryListItem>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCategories", conn))
                {
                    if (getAll)
                    {
                        cmd.Parameters.Add(new SqlParameter("@All", true));
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        int currentId = -1;
                        int lastId = -1;
                        int i = -1;

                        while (reader.Read())
                        {
                            lastId = reader.GetInt32(0);
                            if (lastId != currentId)
                            {
                                if (i >= 0)
                                {
                                    categories[i].SubCategories = categories[i].SubCategories.OrderBy(x => x.SubCategories).ToList();
                                }

                                i++;

                                currentId = lastId;
                                CategoryListItem parent = new CategoryListItem()
                                {
                                    Id = currentId,
                                    Name = reader.GetString(1),
                                    OrderId = reader.GetInt32(2),
                                    SubCategories = new List<CategoryListItem>()
                                };

                                categories.Add(parent);
                            }

                            if (!reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5))
                            {
                                CategoryListItem child = new CategoryListItem()
                                {
                                    Id = reader.GetInt32(3),
                                    Name = reader.GetString(4),
                                    OrderId = reader.GetInt32(5)
                                };

                                categories[i].SubCategories.Add(child);
                            }
                        }
                    }

                    conn.Close();
                }
            }

            return categories.OrderBy(x => x.OrderId).ToList();
        }

        public static List<CategoryListItem> GetCategoriesTags()
        {
            List<CategoryListItem> categories = new List<CategoryListItem>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCategoriesTags", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new CategoryListItem()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }

                    conn.Close();
                }
            }

            return categories;
        }

        public static int AddMainCategory(string catName, out int orderId)
        {
            int id = 0;
            orderId = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddMainCategory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Name", catName));

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = (int)reader.GetDecimal(0);
                            orderId = reader.GetInt32(1);
                        }
                    }

                    conn.Close();
                }
            }


            return id;
        }

        public static int AddSubCategory(string subCatName, string mainCatName, int mainCatId, out int orderId)
        {
            orderId = 0;
            int subCatIdValue = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddSubCategory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@catName", subCatName));
                    cmd.Parameters.Add(new SqlParameter("@mainCatId", mainCatId));
                    cmd.Parameters.Add(new SqlParameter("@mainCatName", mainCatName));

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            subCatIdValue = (int)((decimal)reader[1]);
                            orderId = (int)reader[0];
                        }
                    }

                    conn.Close();
                }
            }

            return subCatIdValue;
        }

        public static string GetAboutContent()
        {
            string aboutText = string.Empty;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAboutContent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            aboutText += "<div class=\"paragraph\">";
                            aboutText += reader.GetString(1);
                            aboutText += "</div>";
                        }
                    }

                    conn.Close();
                }
            }

            return aboutText;
        }

        public static void UpdateAboutContent(string aboutText)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateAboutContent", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Content", aboutText));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static List<Product> GetProductsByCriteria(ProductsCriteria criteria, out CategoryListItem nextCategory)
        {
            nextCategory = null;
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductsByCriteria", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (criteria.CategoryId != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@MainCat", criteria.CategoryId.Value));
                    }

                    cmd.Parameters.Add(new SqlParameter("@PageNumber", criteria.PageNumber));
                    cmd.Parameters.Add(new SqlParameter("@PageSize", 9999));

                    if (criteria.ProductId != null && criteria.ProductId.Value > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@ProductId", criteria.ProductId.Value));
                    }

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product p = new Product()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(3),
                                ImageName = reader.GetString(1),
                                SmallImageName = reader.GetString(5),
                                CategoryId = reader.GetInt32(6),
                                SubCategoryId = reader.GetInt32(2),
                                CategoryName = reader.GetString(7),
                                SubCategoryName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                PreviousProductId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                NextProductId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                            };

                            products.Add(p);
                        }

                        if (criteria.CategoryId != null && reader.NextResult())
                        {
                            if (reader.Read())
                            {
                                nextCategory = new CategoryListItem()
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };
                            }
                        }
                    }

                    conn.Close();
                }
            }

            return products;
        }

        public static Product GetProduct(int productId, out List<Product> similarProducts)
        {
            Product product = null;
            similarProducts = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProductId", productId));

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(3),
                                ImageName = reader.GetString(1),
                                SmallImageName = reader.GetString(5),
                                CategoryId = reader.GetInt32(6),
                                SubCategoryId = reader.GetInt32(2),
                                CategoryName = reader.GetString(7),
                                SubCategoryName = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty,
                                PreviousProductId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                NextProductId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                            };
                        }

                        if (reader.NextResult() && reader.Read())
                        {
                            product.Name = reader.GetString(0);
                        }

                        if (reader.NextResult())
                        {
                            similarProducts = new List<Product>();
                            while (reader.Read())
                            {
                                Product p = new Product()
                                {
                                    Id = reader.GetInt32(0),
                                    SmallImageName = reader.GetString(1),
                                    Name = reader.GetString(2)
                                };

                                similarProducts.Add(p);
                            }
                        }

                        if (reader.NextResult())
                        {
                            product.OrderProducts = new List<OrderProduct>();
                            int prodId = -1;
                            int subProdId = -1;
                            int prodIndex = -1;
                            int propIndex = -1;

                            while (reader.Read())
                            {
                                int currentProdId = reader.GetInt32(6);

                                if (prodId != currentProdId)
                                {
                                    propIndex = -1;
                                    prodIndex++;
                                    prodId = currentProdId;
                                    OrderProduct op = new OrderProduct();
                                    op.Id = prodId;
                                    op.Name = reader.GetString(5);
                                    op.Properties = new List<ProductProperty>();
                                    op.SubProducts = new List<SubProduct>();
                                    product.OrderProducts.Add(op);
                                }

                                int currentSubProdId = reader.GetInt32(0);
                                if (currentSubProdId != subProdId)
                                {
                                    subProdId = currentSubProdId;
                                    propIndex++;
                                    SubProduct sp = new SubProduct();
                                    sp.Id = subProdId;
                                    sp.Name = reader.GetString(4);
                                    sp.PropertiesValues = new List<ProductProperty>();
                                    sp.SKU = reader.GetString(1);
                                    product.OrderProducts[prodIndex].SubProducts.Add(sp);
                                }

                                int propId = reader.GetInt32(10);
                                string propName = reader.GetString(11);

                                var prop = new ProductProperty() { Id = propId, Name = propName };

                                if (!product.OrderProducts[prodIndex].Properties.Any(x => x.Id == propId))
                                {
                                    product.OrderProducts[prodIndex].Properties.Add(prop);
                                }

                                product.OrderProducts[prodIndex].SubProducts[propIndex].PropertiesValues.Add(prop);

                            }
                        }
                    }

                    conn.Close();
                }
            }

            return product;
        }

        public static List<PictureProduct> GetPictureProducts(int? category = null, string term = null)
        {
            List<PictureProduct> products = new List<PictureProduct>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductsFromManagement", conn))
                {
                    if (category.HasValue && category.Value > 0)
                        cmd.Parameters.Add(new SqlParameter("@Cat", category.Value));
                    if (!string.IsNullOrWhiteSpace(term))
                        cmd.Parameters.Add(new SqlParameter("@Term", term));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PictureProduct p = new PictureProduct();
                            p.Id = reader.GetInt32(0);
                            p.ImageName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            p.SubCategoryId = reader.GetInt32(2);
                            p.Title = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            p.SubCategoryName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            p.SmallImageName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                            p.MainCategoryId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                            p.MainCategoryName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                            p.OrderId = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                            p.IsActive = reader.GetBoolean(9);

                            products.Add(p);
                        }
                    }

                    conn.Close();
                }
            }

            return products;
        }

        public static int AddProductPicture(string title, string smallPic, string bigPic, int subCatId, string subCatName, int mainCatId, string mainCatName, out int orderId)
        {
            orderId = 0;
            int id = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddProductPicture", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Title", title));
                    cmd.Parameters.Add(new SqlParameter("@SmallPic", smallPic));
                    cmd.Parameters.Add(new SqlParameter("@BigPic", bigPic));
                    cmd.Parameters.Add(new SqlParameter("@SubCatId", subCatId));
                    cmd.Parameters.Add(new SqlParameter("@SubCatName", subCatName));
                    cmd.Parameters.Add(new SqlParameter("@MainCatId", mainCatId));
                    cmd.Parameters.Add(new SqlParameter("@MainCatName", mainCatName));

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = (int)reader.GetDecimal(0);
                            orderId = reader.GetInt32(1);
                        }
                    }

                    conn.Close();
                }
            }

            return id;
        }

        public static List<ProductTable> GetProductTables(int picId, string term = null)
        {
            List<ProductTable> tables = new List<ProductTable>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductTables", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@PicId", picId));
                    if (!string.IsNullOrWhiteSpace(term))
                        cmd.Parameters.Add(new SqlParameter("@Term", term));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTable p = new ProductTable();
                            p.Id = reader.GetInt32(0);
                            p.Title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            p.PictureId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            p.PictureName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            p.SubCategoryId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                            p.SubCategoryName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                            p.MainCategoryId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                            p.MainCategoryName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                            tables.Add(p);
                        }
                    }

                    conn.Close();
                }
            }

            return tables;
        }

        public static List<SubProductRow> GetSubProducts(int tableId, string term = null)
        {
            List<SubProductRow> subProds = new List<SubProductRow>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSubProducts", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ProductTableId", tableId));
                    if (!string.IsNullOrWhiteSpace(term))
                        cmd.Parameters.Add(new SqlParameter("@Term", term));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubProductRow sp = new SubProductRow();
                            sp.Id = reader.GetInt32(0);
                            sp.SKU = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            sp.ProductPictureTitle = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            sp.ProductPictureId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            sp.SubProductName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            sp.SubCategoryName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                            sp.ProductTableId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                            sp.Description = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                            sp.SubCategoryId = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                            sp.MainCategoryId = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                            sp.MainCategoryName = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                            sp.OrderId = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                            subProds.Add(sp);
                        }
                    }

                    conn.Close();
                }
            }

            return subProds;
        }

        public static int AddSubProduct(string sbName, string sku, int picId, int ptid, int subCatId, int mainCatId)
        {
            int id = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddProductTableRow", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ItemName", sbName));
                    cmd.Parameters.Add(new SqlParameter("@SKU", sku));
                    cmd.Parameters.Add(new SqlParameter("@PicId", picId));
                    cmd.Parameters.Add(new SqlParameter("@ProductTableId", ptid));
                    cmd.Parameters.Add(new SqlParameter("@SubCatId", subCatId));
                    cmd.Parameters.Add(new SqlParameter("@MainCatId", mainCatId));

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }
                    }

                    conn.Close();
                }
            }

            return id;
        }

        public static List<SubProductProperty> GetSubProductProperties(int subCatId)
        {
            List<SubProductProperty> props = new List<SubProductProperty>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductProperties", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@SubProductId", subCatId));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubProductProperty p = new SubProductProperty();
                            p.Id = reader.GetInt32(1);
                            p.Name = reader.GetString(0);
                            props.Add(p);
                        }
                    }

                    conn.Close();
                }
            }

            return props;
        }

        public static int AddSubProductProperty(int subCatId, string propName)
        {
            int id = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddProductProperty", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SubProductId", subCatId));
                    cmd.Parameters.Add(new SqlParameter("@PropName", propName));

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }
                    }

                    conn.Close();
                }
            }

            return id;
        }

        public static int AddSubProductTable(int? subCatId, int picId, string tableName)
        {
            int id = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddProductTable", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PicId", picId));
                    cmd.Parameters.Add(new SqlParameter("@TableName", tableName));

                    if (subCatId.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@SubCatId", subCatId.Value));

                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = (int)reader.GetDecimal(0);
                        }
                    }

                    conn.Close();
                }
            }

            return id;
        }

        public static OrderDay GetOrderDay(int userId)
        {
            string ORDER_DAY = "OrderDay_" + userId;

            if (HttpContext.Current.Session[ORDER_DAY] == null)
            {
                OrderDay orderDay = null;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetDayOrder", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                orderDay = new OrderDay();
                                orderDay.Id = reader.GetInt32(0);
                                orderDay.Date = reader.GetDateTime(1);
                                orderDay.IsApproved = reader.GetBoolean(2);
                                orderDay.IsSent = reader.GetBoolean(3);
                                orderDay.Message = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            }
                        }

                        conn.Close();
                    }
                }

                HttpContext.Current.Session[ORDER_DAY] = orderDay;
            }

            return (OrderDay)HttpContext.Current.Session[ORDER_DAY];
        }

        public static Cart GetCart(int userId, int orderId = 0)
        {
            Cart cart = new Cart();
            cart.Items = new List<CartItem>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCart", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserRecordId", userId));
                    cmd.Parameters.Add(new SqlParameter("@PrevOrderId", orderId));
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cart.OrderId = reader.GetInt32(0);
                            cart.OrderDate = reader.GetDateTime(1);

                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    CartItem item = new CartItem();
                                    item.SKU = reader.GetString(0);
                                    item.Name = reader.GetString(1);
                                    item.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                    item.PicId = reader.GetInt32(3);
                                    item.Quantity = reader.GetInt32(4);
                                    item.Property = reader.GetString(6);
                                    item.Comment = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                                    item.OrderId = reader.GetInt32(8);
                                    cart.Items.Add(item);
                                }
                                if (reader.NextResult() && reader.Read())
                                {
                                    cart.UserMessage = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                                    cart.UserName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                }
                            }
                        }
                    }

                    conn.Close();
                }
            }

            return cart;
        }

        public static void AddProductToCart(Product product, int userId)
        {
            DateTime now = DateTime.Now;
            OrderDay orderDay = GetOrderDay(userId);

            string qTemplate = "INSERT INTO orders(userId," +
                                                    "day," +
                                                    "month," +
                                                    "year," +
                                                    "product_id,property_id," +
                                                    "makat,quantity," +
                                                    "full_order_id," +
                                                    "full_date)" +
                                            "VALUES (" + userId + "," +
                                                    now.Day.ToString() + "," +
                                                    now.Month.ToString() + "," +
                                                    now.Year.ToString() + "," +
                                                    "{0}," + //@product_id
                                                    "N'{1}'," + // @property_id
                                                    "N'{2}'," + // @makat
                                                    "{3}," + //@quantity
                                                    "{4}," + //@full_order_id
                                                    "GETDATE())\n";

            string query = string.Empty;

            foreach (var op in product.OrderProducts)
            {
                foreach (var sp in op.SubProducts)
                {
                    var props = sp.PropertiesValues.Where(x => x.Count > 0);
                    if (props != null && props.Count() > 0)
                    {
                        foreach (var prop in props)
                        {
                            query += string.Format(qTemplate, sp.Id, prop.Name, sp.SKU, prop.Count, orderDay.Id);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(query))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

        }

        public static void AddProductToCart(List<ProductItemModel> products, int userId)
        {
            DateTime now = DateTime.Now;
            OrderDay orderDay = GetOrderDay(userId);

            string qTemplate = "INSERT INTO orders(userId," +
                                                    "day," +
                                                    "month," +
                                                    "year," +
                                                    "product_id,property_id," +
                                                    "makat,quantity," +
                                                    "full_order_id," +
                                                    "full_date)" +
                                            "VALUES (" + userId + "," +
                                                    now.Day.ToString() + "," +
                                                    now.Month.ToString() + "," +
                                                    now.Year.ToString() + "," +
                                                    "{0}," + //@product_id
                                                    "N'{1}'," + // @property_id
                                                    "N'{2}'," + // @makat
                                                    "{3}," + //@quantity
                                                    "{4}," + //@full_order_id
                                                    "GETDATE())\n";

            string query = string.Empty;
            foreach (var item in products)
            {
                query += string.Format(qTemplate, item.Id, item.Property, item.SKU, item.Amount, orderDay.Id);
            }

            if (!string.IsNullOrEmpty(query))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

        }

        public static void RemoveProductFromCart(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RemoveProductForCart", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static void UpdateCartProduct(CartProduct product)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateProductCart", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@OrderId", product.OrderId));
                    cmd.Parameters.Add(new SqlParameter("@Quantity", product.Quantity));
                    cmd.Parameters.Add(new SqlParameter("@IsChecked", product.IsChecked));
                    cmd.Parameters.Add(new SqlParameter("@Comment", product.Comment));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static int InsertHomeBanner(string title, string subTitle, string imageName, out int orderId)
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertHomePic", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@picTitle", title));
                    cmd.Parameters.Add(new SqlParameter("@picTxt", subTitle));
                    cmd.Parameters.Add(new SqlParameter("@picName", imageName));

                    SqlParameter outputId = new SqlParameter();
                    outputId.Direction = ParameterDirection.Output;
                    outputId.ParameterName = "@id";
                    outputId.DbType = DbType.Int32;
                    outputId.Size = sizeof(int);
                    cmd.Parameters.Add(outputId);

                    SqlParameter orderIdOut = new SqlParameter();
                    orderIdOut.Direction = ParameterDirection.Output;
                    orderIdOut.ParameterName = "@max_id";
                    orderIdOut.DbType = DbType.Int32;
                    orderIdOut.Size = sizeof(int);
                    cmd.Parameters.Add(orderIdOut);


                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    id = (int)cmd.Parameters["@id"].Value;
                    orderId = (int)cmd.Parameters["@max_id"].Value;
                    conn.Close();
                }
            }

            return id;
        }

        public static void RemoveHomeBanner(int bannerId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RemoveHomeBanner", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@picId", bannerId));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static List<SiteUser> GetUsers(string userId = null)
        {
            List<SiteUser> users = new List<SiteUser>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetUsers", conn))
                {
                    if (!string.IsNullOrEmpty(userId))
                        cmd.Parameters.Add(new SqlParameter("@Id", userId));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SiteUser user = new SiteUser();
                            user.Id = reader.GetString(0);
                            user.Email = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            user.Phone = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            user.Name = reader.GetString(3);
                            user.UserName = reader.GetString(4);
                            user.RecordId = reader.GetInt32(5);
                            users.Add(user);
                        }
                    }

                    conn.Close();
                }
            }

            return users;
        }

        public static void RemoveOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RemoveOrder", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public static void ApproveOrder(int orderId, string msg, string username)
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RemoveOrder", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                    cmd.Parameters.Add(new SqlParameter("@UserMessage", msg));
                    cmd.Parameters.Add(new SqlParameter("@UserName", username));

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
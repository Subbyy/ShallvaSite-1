using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string SmallImageName { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public int? PreviousProductId { get; set; }
        public int? NextProductId { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }

    public partial class OrderProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductProperty> Properties { get; set; }
        public List<SubProduct> SubProducts { get; set; }
    }

    public partial class SubProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductProperty> PropertiesValues { get; set; }
        public string SKU { get; set; }
    }

    public partial class ProductProperty
    {
        public int Id { get; set; }
        public int? Count { get; set; }
        public string Name { get; set; }
    }
}

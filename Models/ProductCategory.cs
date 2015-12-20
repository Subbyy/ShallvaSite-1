using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public List<ProductCategory> SubCategories { get; set; }
    }
}
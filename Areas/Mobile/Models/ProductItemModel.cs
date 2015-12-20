using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Areas.Mobile.Models
{
    public class ProductItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Property { get; set; }
        public string Amount { get; set; }
    }
}
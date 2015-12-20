using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class CartItem
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PicId { get; set; }
        public int Quantity { get; set; }
        public string Property { get; set; }
        public string Comment { get; set; }
        public int OrderId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class CartProduct
    {
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public string Property { get; set; }
        public bool IsChecked { get; set; }
        public string Comment { get; set; }
    }
}
using ShallvaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace ShallvaMVC.Utils
{
    public class Cart
    {
        public List<CartItem> Items { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserMessage { get; set; }
        public string UserName { get; set; }
        public bool IsApproved { get; set; }
    }
}
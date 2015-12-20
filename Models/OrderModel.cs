using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsSended { get; set; }
        public bool IsApprove { get; set; }
    }
}
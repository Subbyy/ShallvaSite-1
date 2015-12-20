using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class OrderDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsSent { get; set; }
        public bool IsApproved { get; set; }
        public string Message { get; set; }
    }
}
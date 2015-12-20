using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class Workshops
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Instructor { get; set; }
        public decimal Price { get; set; }
        public byte Type { get; set; }
    }
}
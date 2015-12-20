using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class BennerGalleryItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageName { get; set; }
    }
}
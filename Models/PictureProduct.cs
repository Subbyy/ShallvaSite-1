using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class PictureProduct
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public int SubCategoryId { get; set; }
        public string Title { get; set; }
        public string SubCategoryName { get; set; }
        public string SmallImageName { get; set; }
        public int MainCategoryId { get; set; }
        public string MainCategoryName { get; set; }
        public int OrderId { get; set; }
        public bool IsActive { get; set; }
    }
}
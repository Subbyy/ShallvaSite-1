using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class ProductTable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PictureId { get; set; }
        public string PictureName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int MainCategoryId { get; set; }
        public string MainCategoryName { get; set; }
    }
}
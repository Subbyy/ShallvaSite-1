using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class SubProductRow
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string ProductPictureTitle { get; set; }
        public int ProductPictureId { get; set; }
        public string SubProductName { get; set; }
        public string SubCategoryName { get; set; }
        public int ProductTableId { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public int MainCategoryId { get; set; }
        public string MainCategoryName { get; set; }
        public int OrderId { get; set; }
    }
}
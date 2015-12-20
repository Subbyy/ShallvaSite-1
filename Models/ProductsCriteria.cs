
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class ProductsCriteria
    {
        public ProductsCriteria()
        {
            PageNumber = 1;
            PageSize = 20;
        }

        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? ProductId { get; set; }
    }
}
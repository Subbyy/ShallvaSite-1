﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class SiteUser
    {
        public int RecordId { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class ContactModal
    {
        [Required(ErrorMessage = "אנא הזן שם")]
        [Display(Name = "שם:")]
        public string Name { get; set; }
         
        [Display(Name = "טלפון:")]
        [Required(ErrorMessage = "אנא הזן מספר טלפון")]
        [RegularExpression(@"^([0][57][0-9][-]?[0-9]{7})|([0][1234689][-]?[0-9]{7})$", ErrorMessage = "מספר טלפון שגוי")]
        public string Phone { get; set; }

        [Display(Name = "כתובת אימייל:")]
        [Required(ErrorMessage = "אנא הזן כתובת אימייל")]
        [EmailAddress(ErrorMessage = "Example: username@gmail.com")]
        public string Email { get; set; }
    }
}
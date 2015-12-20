using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShallvaMVC.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "אנא הזן שם")]
        [Display(Name = "שם:")]
        public string Name { get; set; }

        [Display(Name = "טלפון:")]
        [Required(ErrorMessage = "אנא הזן מספר טלפון")]
        [RegularExpression(@"^([0][57][0-9][-]?[0-9]{7})|([0][1234689][-]?[0-9]{7})$", ErrorMessage = "מספר טלפון שגוי")]
        public string Phone { get; set; }

        [Display(Name = "טלפון נייד (לא חובה):")]
        [RegularExpression(@"^([0][57][0-9][-]?[0-9]{7})|([0][1234689][-]?[0-9]{7})$", ErrorMessage = "מספר טלפון שגוי")]
        public string Mobile { get; set; }

        [Display(Name = "כתובת אימייל (לא חובה):")]
        [EmailAddress(ErrorMessage = "Example: username@gmail.com")]
        public string Email { get; set; }

        [Display(Name = "שם העסק (לא חובה):")]
        public string BusinessName { get; set; }

        [Display(Name = "ת.ז / ע.מ / ח.פ (לא חובה):")]
        public string Id { get; set; }

        [Display(Name = "כתובת העסק (לא חובה):")]
        public string BusinessAddress { get; set; }

        [Display(Name = "נושא ההודעה:")]
        [Required(ErrorMessage = "אנא הזן את נושא ההודעה")]
        public string Subject { get; set; }

        [Display(Name = "הודעה:")]
        [Required(ErrorMessage = "אנא רשום את תוכן ההודעה")]
        public string Content { get; set; }
    }
}
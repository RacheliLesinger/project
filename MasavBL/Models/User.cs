using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasavBL.Models
{
    public class User
    {
        [DisplayName("מזהה")]
        public int Id { get; set; }
        [DisplayName("שם משתמש")]
       
        [Required(ErrorMessage = "חובה להכניס שם משתמש")]
        public string UserName { get; set; }
        [DisplayName("סיסמה")]
        [Required(ErrorMessage = "חובה להכניס סיסמה")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("רמת הרשאה")]
        [Required(ErrorMessage = "חובה לבחור רמת הרשאה")]
        public string Role { get; set; }
    }
}
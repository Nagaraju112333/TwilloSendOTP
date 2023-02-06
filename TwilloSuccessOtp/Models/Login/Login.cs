using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace TwilloSuccessOtp.Models.Login
{
    public class Login
    {
        [Required (ErrorMessage ="Enter Username")]
     
        public string email { get; set; }
        [Required (ErrorMessage ="Enter Password")]
       
        public string Password { get; set; }    
    }
}
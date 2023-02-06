using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwilloSuccessOtp.Models;
using TwilloSuccessOtp.Models.Login;
using TwilloSuccessOtp.Models.ForgotPassword;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TwilloSuccessOtp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        TwilloEntities2 db=new TwilloEntities2();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
    
        [HttpPost]
      
        public ActionResult Login(Login User)
        {
            string message = "";
            if (ModelState.IsValid)
            {
               // var v = dc.UserRegisters.Where(a => a.Email == emailID && a.Password==password).FirstOrDefault();
                var result = db.UserRegisters.Where(x => x.Email == User.email && x.Password == User.Password).FirstOrDefault();
                if (result!=null)
                {
                    return RedirectToAction("GetDetails", "Login");
                }
                else
                {
                    message = "Invalid credential provided";
                }

             /*   ModelState.AddModelError("Worng", "UserName And Password Wrong");*/
            }
            ViewBag.Message=message;
            return View();
        }
        [HttpGet]
        public ActionResult GetDetails()
        {
            var result = db.UserRegisters.ToList();
            return View(result);
        }
        /*   public bool IsEmailExist(string emailID ,string password)
           {
               using (TwilloEntities2 dc = new TwilloEntities2())
               {
                   var v = dc.UserRegisters.Where(a => a.Email == emailID && a.Password==password).FirstOrDefault();
                   return v != null;
               }
           }*/
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            var result = db.UserRegisters.Select(x => x.Phone_Number);
            ViewBag.Number = result;
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(UserRegister Password)
        {
            if(ModelState.IsValid)
            {
                var resul = ViewBag.Number;
                var result = db.UserRegisters.FirstOrDefault(x => x.Phone_Number == Password.Phone_Number);
                if (result != null)
                {
                    int otpValue = new Random().Next(100000, 999999);
                    const string accountSid = "AC48f2191188263b7f3b4eb5d1c9c0bd95";
                    const string authToken = "83d6589f4dc53d60022984faa209cc69";

                    TwilioClient.Init(accountSid, authToken);

                    //https://support.twilio.com/hc/en-us/articles/223183068-Twilio-international-phone-number-availability-and-their-capabilities
                    var to = new PhoneNumber("+91" + Password.Phone_Number);

                    var message = MessageResource.Create(
                        to,
                        //First navigate to your test credentials https://www.twilio.com/user/account/developer-tools/test-credentials
                        // then you need to get Twilio number from https://www.twilio.com/console/voice/numbers
                        from: new PhoneNumber("+13602275378"), //  From number, must be an SMS-enabled Twilio number ( This will send sms from ur "To" numbers ). 
                        body: $"Verification OTP "+ otpValue + "");
                     Password.OTP = otpValue;
                    db.SaveChanges();
                }

            }

            return View();
        }


    }
}
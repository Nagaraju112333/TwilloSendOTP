using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using Twilio.Types;
using TwilloSuccessOtp.Models;

namespace TwilloSuccessOtp.Controllers
{
    public class UserRegisterController : Controller
    {
       

       /* private readonly ITwilioRestClient _client;
        public UserRegister(ITwilioRestClient client)
        {
            _client = client;
        }*/
        TwilloEntities2 db = new TwilloEntities2();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
       public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserRegister register)
        {
            var AccountSid = "AC48f2191188263b7f3b4eb5d1c9c0bd95";
            var AuthToken = "83d6589f4dc53d60022984faa209cc69";
            var aid = "";
            var sid = "";
            
            int otpValue = new Random().Next(100000, 999999);
            var Message = "Your otp";
            var from = "+13602275378";
            var message = MessageResource.Create(
               to: new PhoneNumber(register.Phone_Number),
              from: new PhoneNumber(from),
               body:Message+otpValue
                );
            return View(register);
        }
        [HttpPost]
        public ActionResult UserRegister(UserRegister user)
        {
          
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
             // user.RegisterId=Guid.NewGuid();
                // Find your Account Sid and Auth Token at twilio.com/console
                const string accountSid = "AC48f2191188263b7f3b4eb5d1c9c0bd95";
                const string authToken = "83d6589f4dc53d60022984faa209cc69";

                TwilioClient.Init(accountSid, authToken);

                //https://support.twilio.com/hc/en-us/articles/223183068-Twilio-international-phone-number-availability-and-their-capabilities
                var to = new PhoneNumber("+91" + user.Phone_Number);

                var message = MessageResource.Create(
                    to,
                    //First navigate to your test credentials https://www.twilio.com/user/account/developer-tools/test-credentials
                    // then you need to get Twilio number from https://www.twilio.com/console/voice/numbers
                    from: new PhoneNumber("+13602275378"), //  From number, must be an SMS-enabled Twilio number ( This will send sms from ur "To" numbers ). 
                    body: $"Your Account Successfully creted "+"UserName:" + user.Email +"Password:"+ user.Password+"");

                db.UserRegisters.Add(user);
                db.SaveChanges();
                ViewBag.Message = "Message Sent";
                return RedirectToAction("Login","login");
            }
            else
            {
                return View();
            }

        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (TwilloEntities2 dc = new TwilloEntities2())
            {
                var v = dc.UserRegisters.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }

    }
}
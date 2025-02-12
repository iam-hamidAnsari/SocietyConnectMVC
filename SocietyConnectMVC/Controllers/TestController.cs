using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SocietyConnectMVC.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private const string AccountSid = "AC0afa4843417b1046eef3beb4dde0a1b9";
        private const string AuthToken = "0fc441de8ac9661880384ea8a2be1081";  
        private const string FromWhatsAppNumber = "whatsapp:+17752577285";

        [HttpPost]
        public IActionResult Index(string no)
        {
            try
            {
                TwilioClient.Init(AccountSid, AuthToken);

                var toWhatsAppNumber = new PhoneNumber($"whatsapp:{no}");

                var messageOptions = new CreateMessageOptions(toWhatsAppNumber)
                {
                    From = new PhoneNumber(FromWhatsAppNumber),
                    Body = "Here Is Your Report!!"
                };

                //// Add attachment if provided
                //if (!string.IsNullOrEmpty(mediaUrl))
                //{
                //    messageOptions.MediaUrl = new List<Uri> { new Uri(mediaUrl) };
                //}

                //messageOptions.MediaUrl = new List<Uri> { new Uri("https://postimg.cc/30C9Mjh1") };

                var msg = MessageResource.Create(messageOptions);

                TempData["sucess"] = "Msg Send Successfully!!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

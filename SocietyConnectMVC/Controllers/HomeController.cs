using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.Users;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Migrations;
using SocietyConnectMVC.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

namespace SocietyConnectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string name,string email, string subject,string message)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add("mpoke1928@gmail.com");
            mail.From = new MailAddress("mpoke1928@gmail.com");
            mail.Subject = subject;
            mail.Body = $"User Email is {email}\nhere is detailed message\n{message}";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("mpoke1928@gmail.com", "dhfx widi jvps itnr");
            smtp.EnableSsl = true;

            smtp.Send(mail);
            TempData["sucess"] = "Details Send Successfully!!";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            if (email != null && pass != null)
            {
                var check = db.users.FirstOrDefault(u => u.Email.Equals(email) && u.Password.Equals(pass));
                if (check != null)
                {
                    HttpContext.Session.SetString("UserEmail", check.Email);
                    HttpContext.Session.SetString("UserName", check.Name);
                    if (check.role.Equals("user"))
                    {
                        TempData["sucess"] = $"Welcome  {email}";
                        return RedirectToAction("UserDb","DashBoard");
                    }
                    else if (check.role.Equals("admin"))
                    {
                        TempData["sucess"] = $"Welcome {email}";
                        return RedirectToAction("AdminDb", "DashBoard");
                    }
                }
            }
            TempData["error"] = "Invalid Credentials!!";
            return View();
        }

        public async Task OAuthLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var email = result.Principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var check = db.users.Where(a => a.Email.Equals(email)).SingleOrDefault();
            if (check != null)
            {
                HttpContext.Session.SetString("UserEmail", check.Email);
                TempData["sucess"] = $"Welcome {email}";
                return RedirectToAction("UserDb", "DashBoard");
            }

            return RedirectToAction("Login");

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

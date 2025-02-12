using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Migrations;
using SocietyConnectMVC.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using System.Globalization;
using Microsoft.VisualStudio.Services.WebApi;

namespace SocietyConnectMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        public UsersController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        public IActionResult Index()
        {
            var data = db.users.Where(a => a.role.Equals("user")).ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (search != null)
            {
                var data = db.users.Where(a => a.role.Contains(search) || a.Name.Contains(search) || a.Email.Contains(search) || a.CreatedAt.Contains(search)).ToList();
                return View(data);
            }
            else
            {
                var data = db.users.Where(a => a.role.Equals("user")).ToList();
                return View(data);
            }
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User u)
        {

            User user;
            user = new User
            {
                Name = u.Name,
                Email = u.Email,
                role = "user",
                Password = u.Password,
                CreatedAt = DateTime.Now.ToString("dd-MM-yyyy"),
                profile_image = null
            };
            db.users.Add(user);
            db.SaveChanges();
            MailMessage mail = new MailMessage();
            mail.To.Add(user.Email);
            mail.From = new MailAddress("mpoke1928@gmail.com");
            mail.Subject = "Welcome To Society Connect.";
            mail.Body = $"Here is Your Login Credentials\n" +
                $"Here is all details\n" +
                $"Name : {user.Name}\n" +
                $"Email : {user.Email}\n" +
                $"Password : {user.Password} \n" +
                $"Login Using Email and Password." +
                $"regards,\n" +
                $"Society Connect.";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("mpoke1928@gmail.com", "dhfx widi jvps itnr");
            smtp.EnableSsl = true;

            smtp.Send(mail);
            TempData["sucess"] = "User Added Successfully!!";
            return RedirectToAction("Index");
        }

        public IActionResult ViewUser(int id)
        {
            var data = db.users.Find(id);
            return View(data);
        }

        
        public IActionResult EditUser(int id)
        {
            var data = db.users.Find(id);
            return View(data); 
        }

        [HttpPost]
        public IActionResult EditUser(User u,string pass)
        {
            User user;
            if (!string.IsNullOrEmpty(pass))
            {
                user = new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    role = "user",
                    Password =  pass,
                    //Password = u.Password,
                    CreatedAt = u.CreatedAt,
                    profile_image = u.profile_image
                };
            }
            else 
            {
                var password = db.users.Where(x => x.Id.Equals(u.Id)).Select(x => x.Password).SingleOrDefault();
                user = new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    role = "user",
                    Password = password,
                    CreatedAt = u.CreatedAt,
                    profile_image = u.profile_image
                };
            }
            db.users.Update(user);
            db.SaveChanges();
            TempData["update"] = "User Profile Updated Successfully!!";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteUser(int id)
        {
            var data = db.users.Find(id);
            db.users.Remove(data);
  
            var billdata = db.bills.Where(x => x.user_email == data.Email).ToList();
            if (billdata != null)
            {
                db.bills.RemoveRange(billdata);
            }

            var allotflat = db.Flat_Alltmnt.Where(a => a.AllottedTo.Equals(data.Email)).SingleOrDefault();
            if (allotflat != null)
            {
                var visitordata = db.visitors.Where(x => x.alloted_flats.ToString() == allotflat.FlatNo).ToList();
                if (visitordata != null)
                {
                    db.visitors.RemoveRange(visitordata);
                }
                var complaintdata= db.Complaints.Where(x => x.User_Email == data.Email).ToList();
                if (complaintdata != null)
                {
                    db.Complaints.RemoveRange(complaintdata);
                }

                db.Flat_Alltmnt.Remove(allotflat);
            }
            db.SaveChanges();
            TempData["error"] = "User Deleted Successfully!!";
            return RedirectToAction("Index");
        }

        public IActionResult ManageUser()
        {
            string email = HttpContext.Session.GetString("UserEmail");
            var data = db.users.Where(a => a.Email.Equals(email)).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult ManageUser(User u,string pass)
        {
            User user;
            if (!string.IsNullOrEmpty(pass))
            {
                user = new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    role = "user",
                    Password = pass,
                    //Password = u.Password,
                    CreatedAt = u.CreatedAt,
                    profile_image = u.profile_image
                };
            }
            else
            {
                var password = db.users.Where(x => x.Id.Equals(u.Id)).Select(x => x.Password).SingleOrDefault();
                user = new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    role = "user",
                    Password = password,
                    CreatedAt = u.CreatedAt,
                    profile_image = u.profile_image
                };
            }
            db.users.Update(user);
            db.SaveChanges();
            TempData["update"] = "User Profile Updated Successfully!!";
            return RedirectToAction("UserDb","DashBoard");
        }
        private void UploadFile(IFormFile file, string fpath)
        {
            FileStream stream = new FileStream(fpath, FileMode.Create);
            file.CopyTo(stream);
        }
    }
}

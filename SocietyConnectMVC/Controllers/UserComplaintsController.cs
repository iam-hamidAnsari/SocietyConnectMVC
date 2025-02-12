using Microsoft.AspNetCore.Mvc;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;

namespace SocietyConnectMVC.Controllers
{
    public class UserComplaintsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserComplaintsController(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        public IActionResult Index()
        {

            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Home");
            }
            var complaints = _db.Complaints.Where(c => c.User_Email == userEmail).ToList();


            return View(complaints);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {

            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Home");
            }
            if(search == null)
            {
                var complaints = _db.Complaints.Where(c => c.User_Email == userEmail).ToList();
                return View(complaints);
            }
            var data = _db.Complaints.Where(c => c.User_Email == userEmail).ToList();
            var search1 = data.Where(a=> a.Description.Contains(search) || a.AllotedFlat.ToString().Contains(search) || a.ResolvedAt.Contains(search)).ToList();


            return View(search1);
        }

        public IActionResult ViewComplaint(int id)
        {
            var complaint = _db.Complaints
                .FirstOrDefault(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }



        public IActionResult EditComplaint(int id)
        {

            var complaint = _db.Complaints.FirstOrDefault(c => c.Id == id);
            if (complaint == null)
            {
                return NotFound();
            }
            return View(complaint);
        }


        [HttpPost]
        public IActionResult EditComplaint(Complaint complaint)
        {

            var existingComplaint = _db.Complaints.FirstOrDefault(c => c.Id == complaint.Id);
            if (existingComplaint == null)
            {
                return NotFound();
            }

            existingComplaint.Description = complaint.Description;

            _db.SaveChanges();
            TempData["update"] = "Complaint Details Updated Successfully!!";
            return RedirectToAction("Index");
        }
        public IActionResult DeleteComplaint(int id)
        {

            var data = _db.Complaints.Find(id);
            _db.Complaints.Remove(data);
            _db.SaveChanges();
            TempData["error"] = "Complaint Deleted Successfully!!";
            return RedirectToAction("Index");

        }

        public IActionResult AddComplaint()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddComplaint(Complaint complaint)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
          
            if (userEmail == null)
            {

                ViewBag.Error = "User not found.";
                return View();
            }


            var flatAllotment = _db.Flat_Alltmnt.Where(f => f.AllottedTo == userEmail).Select(a=> a.FlatNo).Take(1).SingleOrDefault();

            if (flatAllotment == null)
            {

                ViewBag.Error = "Flat allotment not found.";
                return View();
            }

            complaint.AllotedFlat = int.Parse(flatAllotment.ToString());
            complaint.Complaint_Type = complaint.Description;
            complaint.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy");
            complaint.User_Email = userEmail;
            complaint.Status = "Pending";


            _db.Complaints.Add(complaint);
            _db.SaveChanges();
            Notification n = new Notification();
            n.Email = "admin@gmail.com";
            n.Message = $"A new Complaint Raise By Flat no {complaint.AllotedFlat}";
            n.Url = $"/Complaints/View/{complaint.Id}";
            _db.notifications.Add(n);
            _db.SaveChanges();
            TempData["sucess"] = "Complaint Raised Successfully!!";
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;

namespace SocietyConnectMVC.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext db;
        public DashBoardController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult UserDb()
        {

            Analytics a = new Analytics();
            string userEmail = HttpContext.Session.GetString("UserEmail");
            a.TotalComplaints = db.Complaints.Where(a=> a.User_Email.Equals(userEmail)).Count();
            a.InProcessComplaints = db.Complaints.Where(a => a.Status.Equals("resolved") && a.User_Email.Equals(userEmail)).Count();
            a.ResolvedComplaints = db.Complaints.Where(a => a.Status.Equals("in progress") && a.User_Email.Equals(userEmail)).Count();
            return View(a);
        }

        public IActionResult AdminDb()
        {
            Analytics a = new Analytics();
            a.TotalBills = db.bills.Count();
            a.TotalFlats = db.flats.Count();
            a.TotalAllotments = db.Flat_Alltmnt.Count();
            a.TotalComplaints = db.Complaints.Count();
            a.InProcessComplaints = db.Complaints.Where(a=> a.Status.Equals("resolved")).Count();
            a.ResolvedComplaints = db.Complaints.Where(a => a.Status.Equals("in progress")).Count();
            return View(a);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Migrations;
using SocietyConnectMVC.Models;

namespace SocietyConnectMVC.Controllers
{
    public class UserVisitorController : Controller
    {
        private readonly ApplicationDbContext db;
        public UserVisitorController(ApplicationDbContext db) 
        { 
            this.db = db;
        }

        public ActionResult ViewUserVisitor()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            string allotment = db.Flat_Alltmnt.Where(f => f.AllottedTo.Equals(userEmail)).Select(a=> a.FlatNo).SingleOrDefault();

            if (allotment != null)
            {
                var visitors = db.visitors
        .Where(v => v.alloted_flats.ToString() == allotment)  // Use '==' for simple value comparison
        .ToList();
                return View(visitors);
            }
            return View();
        }


        public ActionResult Search(string searchTerm)
        {
            
            string userEmail = HttpContext.Session.GetString("UserEmail");
            string allotment = db.Flat_Alltmnt.Where(f => f.AllottedTo.Equals(userEmail)).Select(a => a.FlatNo).SingleOrDefault();
            if (allotment == null)
            {
                return RedirectToAction("ViewUserVisitor");
            }
            if (string.IsNullOrEmpty(searchTerm))
            {
                var data1 = db.visitors.Where(v => v.alloted_flats.ToString() == allotment).ToList();
                return RedirectToAction("ViewUserVisitor", data1);
            }
            var data = db.visitors.Where(v => v.alloted_flats.ToString() == allotment).ToList(); 
            var visitors = data.Where(a=> a.visitor_name.Contains(searchTerm) || a.purpose.Contains(searchTerm) || a.meet_to.Contains(searchTerm) || a.alloted_flats.ToString().Contains(searchTerm)).ToList();
            return View("ViewUserVisitor", visitors);
        }



        public ActionResult Add()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            ViewBag.FlatNumbers = db.Flat_Alltmnt.Where(a=>a.AllottedTo.Equals(userEmail))
                .Select(f => new SelectListItem
                {
                    Value = f.FlatNo.ToString(),
                    Text = f.FlatNo
                }).ToList();

            return View();
        }


        [HttpPost]

        public ActionResult Add(Visitor visitor)
        {
            if (ModelState.IsValid)
            {
                visitor.createdAt = DateTime.Now.ToString("dd-MM-yyyy");
                if (!string.IsNullOrEmpty(visitor.check_in_dt))
                {

                    visitor.check_in_dt = DateTime.ParseExact(visitor.check_in_dt, "yyyy-MM-ddTHH:mm", null).ToString("yyyy-MM-dd HH:mm:ss");
                }

                db.visitors.Add(visitor);
                db.SaveChanges();
                TempData["sucess"] = "Visitor Added Successfully!!";
                return RedirectToAction("ViewUserVisitor");
            }

            return View(visitor);
        }


        public ActionResult Edit(int id)
        {
            var visitor = db.visitors.FirstOrDefault(v => v.id == id);
            if (visitor == null)
            {
                return RedirectToAction("ViewUserVisitor");
            }

            return View(visitor);
        }

        [HttpPost]
        public IActionResult UpdateVisitorCheckOutDate(int VisitorId, DateTime? CheckOutDate)
        {
            if (VisitorId == 0 || CheckOutDate == null)
            {
                ViewBag.Message = "Invalid data.";
                return RedirectToAction("ViewVisitor", new { id = VisitorId });
            }

            var visitor = db.visitors
                .FirstOrDefault(v => v.id == VisitorId);

            if (visitor == null)
            {
                ViewBag.Message = "Visitor not found.";
                return RedirectToAction("Index");
            }
            visitor.check_out_dt = CheckOutDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

            db.SaveChanges();
            ViewBag.Message = "Check-Out Date updated successfully.";

            return RedirectToAction("ViewUserVisitor", new { id = VisitorId });
        }


        [HttpPost]
        public ActionResult Edit(Visitor visitor)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(visitor.check_in_dt))
                {

                    visitor.check_in_dt = DateTime.ParseExact(visitor.check_in_dt, "yyyy-MM-ddTHH:mm", null).ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (!string.IsNullOrEmpty(visitor.check_out_dt))
                {

                    visitor.check_out_dt = DateTime.ParseExact(visitor.check_out_dt, "yyyy-MM-ddTHH:mm", null).ToString("yyyy-MM-dd HH:mm:ss");
                }


                db.Entry(visitor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["update"] = "Visitor Updated Successfully!!";
                return RedirectToAction("ViewUserVisitor");
            }

            return View(visitor);
        }

        public ActionResult View(int id)
        {
            var visitor = db.visitors
       .Where(v => v.id == id).FirstOrDefault();

            return View(visitor);
        }

        public ActionResult Delete(int id)
        {
            var visitor = db.visitors.FirstOrDefault(v => v.id == id);
            if (visitor == null)
                return RedirectToAction();

            db.visitors.Remove(visitor);
            db.SaveChanges();
            TempData["error"] = "Visitor Deleted Successfully!!";
            return RedirectToAction("ViewUserVisitor");
        }
    }
}

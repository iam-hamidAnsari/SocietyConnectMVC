using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;

namespace SocietyConnectMVC.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly ApplicationDbContext db;
        public VisitorsController(ApplicationDbContext db) 
        {
            this.db = db;
        }

        public ActionResult ViewVisitor()
        {
            var visitors = db.visitors.ToList();

            return View(visitors);
        }


        public ActionResult Search(string searchTerm)
        {
            var visitors = string.IsNullOrEmpty(searchTerm)
                ? db.visitors.ToList()
                : db.visitors.Where(v => v.visitor_name.Contains(searchTerm) || v.visitor_contact.Contains(searchTerm)).ToList();

            return View("ViewVisitor", visitors);
        }


        public ActionResult Add()
        {
            ViewBag.FlatNumbers = db.Flat_Alltmnt
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
                return RedirectToAction("ViewVisitor");
            }

            return View("ViewVisitor");
        }


        public ActionResult Edit(int id)
        {
            var visitor = db.visitors.FirstOrDefault(v => v.id == id);
            if (visitor == null)
            {
                return RedirectToAction("ViewVisitor");
            }

            return View(visitor);
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
                return RedirectToAction("ViewVisitor");
            }

            return View(visitor);
        }

        public ActionResult View(int id)
        {

            var visitor = db.visitors
       .Where(v => v.id == id)
       .FirstOrDefault();

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

            return RedirectToAction("ViewVisitor", new { id = VisitorId });
        }

        public ActionResult Delete(int id)
        {
            var visitor = db.visitors.FirstOrDefault(v => v.id == id);
            if (visitor == null)
                return RedirectToAction();

            db.visitors.Remove(visitor);
            db.SaveChanges();
            TempData["error"] = "Visitor Deleted Successfully!!";
            return RedirectToAction("ViewVisitor");
        }

    }
}

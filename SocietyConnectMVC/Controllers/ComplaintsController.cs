using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.TeamFoundation.Build.WebApi;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace SocietyConnectMVC.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ComplaintsController(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        public IActionResult Index()
        {
            var complaints = _db.Complaints.ToList();
            return View(complaints);
        }
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                var complaints1 = _db.Complaints.ToList();
                return View("Index",complaints1);
            }
            var complaints = _db.Complaints
                .Where(c =>
                    c.User_Email.Contains(query) ||
                    c.AllotedFlat.ToString().Contains(query) || 
                    c.Complaint_Type.Contains(query) || 
                    c.Status.Contains(query)) 
                .ToList();
            return RedirectToAction("Index", complaints);
        }

        public IActionResult View(int id)
        {

            var complaint = _db.Complaints.FirstOrDefault(c => c.Id == id); 

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);

        }
        public IActionResult Delete(int id)
        {

            var data = _db.Complaints.Find(id);
            _db.Complaints.Remove(data);
            _db.SaveChanges();
            TempData["error"] = "Complaint Deleted sucessfully!";
            return RedirectToAction("Index");

        }

        public IActionResult Edit(int id)
        {


            var complaint = _db.Complaints.FirstOrDefault(c => c.Id == id);

            if (complaint == null)
            {
                return View("Index");
            }

            ViewBag.FlatNumbers = _db.Flat_Alltmnt
                .Select(f => new SelectListItem
                {
                    Value = f.FlatNo.ToString(),
                    Text = f.FlatNo
                }).ToList();

            return View(complaint);
        }

        [HttpPost]

        public IActionResult Edit(Complaint model)
        {
            


            var complaint = _db.Complaints
                               .Where(c => c.Id == model.Id)
                               .FirstOrDefault();

            if (complaint != null)
            {

                
                complaint.Description = model.Description; 

               
                _db.SaveChanges();

                
                TempData["update"] = "Complaint updated successfully!";
                return RedirectToAction("Index");
            }



            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateStatus(Complaint c)
        {
            var data = _db.Complaints.Find(c.Id);
            data.Status = c.Status;
            if (c.comments != null)
            {
                data.comments = c.comments;
            }
            _db.SaveChanges();
            Notification n = new Notification();
            n.Email = data.User_Email;
            n.Message = $"Your Complaint Status Changed To {c.Status}.";
            n.Url = $"/UserComplaints/ViewComplaint/{data.Id}";
            _db.notifications.Add(n);
            _db.SaveChanges();
            TempData["update"] = "Complaint Status Changed Successfully!!";
            return RedirectToAction("Index");
        }
    }
}

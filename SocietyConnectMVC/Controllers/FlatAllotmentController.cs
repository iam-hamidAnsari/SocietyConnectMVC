using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;

namespace SocietyConnectMVC.Controllers
{
    public class FlatAllotmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FlatAllotmentController(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        public IActionResult Allotments()
        {
            var data = _db.Flat_Alltmnt.ToList();
            return View(data);
        }

        public IActionResult Edit(int id)
        {
            var flatAlltmnt = _db.Flat_Alltmnt.Find(id);

            if (flatAlltmnt == null)
            {
                return NotFound();
            }

            ViewBag.Users = new SelectList(_db.users.Where(a => a.role.Equals("user")).Select(a => a.Email).ToList());

            ViewBag.Flats = new SelectList(_db.flats.Select(a => a.flat_no).ToList());

            return View(flatAlltmnt);
        }
        [HttpPost]
        public IActionResult Edit(FlatAlltmnt flatAlltmnt)
        {
            var data = _db.Flat_Alltmnt.Where(a => a.Id == flatAlltmnt.Id).SingleOrDefault();
            data.MoveOutDt = flatAlltmnt.MoveOutDt;
            data.MoveInDt = flatAlltmnt.MoveInDt;
            data.AllottedTo = flatAlltmnt.AllottedTo;
            _db.SaveChanges();
            TempData["update"] = "Allotment Details Updated Sucessfully!!";
            return RedirectToAction("Allotments");
        }

        public ActionResult Details(int id)
        {
            var data = _db.Flat_Alltmnt
       .Where(v => v.Id == id)
       .FirstOrDefault();
            return View(data);
        }

        public IActionResult Delete(int id)
        {

            var data = _db.Flat_Alltmnt.FirstOrDefault(v => v.Id == id);
            _db.Flat_Alltmnt.Remove(data);
            var billdata = _db.bills.Where(x => x.user_email == data.AllottedTo).SingleOrDefault();
            if (billdata != null)
            {
                _db.bills.Remove(billdata);
            }
            var visitordata = _db.visitors.Where(x => x.alloted_flats.ToString() == data.FlatNo).SingleOrDefault();
            if (visitordata != null)
            {
                _db.visitors.Remove(visitordata);
            }
            var complaintdata = _db.Complaints.Where(x => x.AllotedFlat.ToString() == data.FlatNo).SingleOrDefault();
            if (complaintdata != null)
            {
                _db.Complaints.Remove(complaintdata);
            }
            _db.SaveChanges();
            TempData["error"] = "Allotment Delete Successfully!!";
            return RedirectToAction("Allotments");

        }


        public ActionResult Create()
        {

            ViewBag.Users = new SelectList(_db.users.Where(a => a.role.Equals("user")).Select(a => a.Email).ToList());

            ViewBag.Flats = new SelectList(_db.flats.Where(x=> x.is_allotted.Equals("f")).Select(a => a.flat_no).ToList());

            return View();
        }


        [HttpPost]
        public ActionResult Create(FlatAlltmnt f)
        {
            _db.Flat_Alltmnt.Add(f);
            _db.SaveChanges();
            var data = _db.flats.Where(a=> a.flat_no.Equals(f.FlatNo)).SingleOrDefault();
            data.is_allotted = "t";
            _db.SaveChanges();
            TempData["sucess"] = "FLat Allotted SuccessFully!!";
            return RedirectToAction("Allotments");

        }


    }
}

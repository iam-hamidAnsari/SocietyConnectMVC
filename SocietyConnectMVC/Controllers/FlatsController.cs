using Microsoft.AspNetCore.Mvc;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace SocietyConnectMVC.Controllers
{
    public class FlatsController : Controller
    {
        private readonly ApplicationDbContext db;
        public FlatsController(ApplicationDbContext db) 
        {
            this.db = db;
        }

        public IActionResult Index()
        {

            var flats = db.flats.ToList();
            return View(flats);
        }



        public ActionResult Search(string searchTerm)
        {
            var flat = string.IsNullOrEmpty(searchTerm)
                ? db.flats.ToList()
                : db.flats.Where(v => v.flat_no.Contains(searchTerm) || v.flat_type.Contains(searchTerm) || v.block_no.Contains(searchTerm)).ToList();

            return View("Index", flat);
        }



        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Flat f)
        {
            f.is_allotted = "f";
            db.flats.Add(f);
            db.SaveChanges();
            TempData["sucess"] = "Flat Added Successfully!!";
            return RedirectToAction("Index");

        }


        public ActionResult Delete(int id)
        {
            var data = db.flats.FirstOrDefault(v => v.id == id);
            if (data == null)
                return RedirectToAction();
            db.flats.Remove(data);
            db.SaveChanges();
            TempData["error"] = "Flat Deleted Successfully!!";
            return RedirectToAction("Index");
        }

        public ActionResult View(int id)
        {
            var data = db.flats
       .Where(v => v.id == id)
       .FirstOrDefault();
            return View(data);
        }

        public ActionResult Edit(int id)
        {
            var data = db.flats.FirstOrDefault(v => v.id == id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }

            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Flat f)
        {
            var data = db.flats.Find(f.id);
            data.flat_no = f.flat_no;
            data.floor_no = f.floor_no;
            data.block_no = f.block_no;
            data.flat_type = f.flat_type;
            db.flats.Update(data);
            db.SaveChanges();
            TempData["update"] = "Flat Details Updated Successfully!!";

            return RedirectToAction("Index");
        }

        public IActionResult GetFlatDetails(string flatNo)
        {
            var flat = db.flats.FirstOrDefault(f => f.flat_no == flatNo);
            if (flat == null)
            {
                return NotFound();
            }

            return Json(new
            {
                floorNo = flat.floor_no,
                blockNo = flat.block_no,
                flatType = flat.flat_type
            });
        }
    }
}

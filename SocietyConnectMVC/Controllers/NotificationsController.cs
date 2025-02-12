using Microsoft.AspNetCore.Mvc;
using SocietyConnectMVC.Data;

namespace SocietyConnectMVC.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext db;
        public NotificationsController(ApplicationDbContext db) 
        {
            this.db = db;
        }

        public IActionResult GetNotifications(string email)
        {
            var data = db.notifications.Where(a=> a.Email.Equals(email) && a.Status==0).ToList();
            return new JsonResult(data);
        }

        public IActionResult MarkAsRead(int notificationId)
        {
            var notification = db.notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.Status = 1; 
                db.SaveChanges();
                return Ok(new { success = true });
            }
            return NotFound(new { success = false, message = "Notification not found" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace SocietyConnectMVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext db;
        public ReportController(ApplicationDbContext db) 
        { 
            this.db = db;
        }

        public IActionResult AdminReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminReport(string type, DateTime startdate, DateTime enddate)
        {
            
            var report = new Report();

            if (!string.IsNullOrEmpty(type))
            {
                
                if (type.Equals("bills"))
                {
                    var allBills = db.bills.ToList();
                    var filteredBills = allBills
                        .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                    createdDate >= startdate && createdDate <= enddate)
                        .ToList();
                    report.Bills = filteredBills;
                }
                
                else if (type.Equals("visitors"))
                {
                    var allVisitors = db.visitors.ToList();
                    var filteredVisitors = allVisitors
                        .Where(b => DateTime.TryParse(b.createdAt, out DateTime createdDate) &&
                                    createdDate >= startdate && createdDate <= enddate)
                        .ToList();
                    report.Visitors = filteredVisitors;
                }
               
                else if (type.Equals("complaints"))
                {
                    var allComplaints = db.Complaints.ToList();
                    var filteredComplaints = allComplaints
                        .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                    createdDate >= startdate && createdDate <= enddate)
                        .ToList();
                    report.Complaints = filteredComplaints;
                }

                
                ViewData["StartDate"] = startdate.ToString("yyyy-MM-dd");
                ViewData["EndDate"] = enddate.ToString("yyyy-MM-dd");

            
                return View(report);
            }

            return View();
        }


        public IActionResult AdminExportToCsv(string type, DateTime startdate, DateTime enddate)
        {
            if (!string.IsNullOrEmpty(type) && type.Equals("bills"))
            {
                var allBills = db.bills.ToList();
                var filteredBills = allBills
                    .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("ID,Flat No,Bill Type,Amount,Month,Created At");

                foreach (var bill in filteredBills)
                {
                    sb.AppendLine($"{bill.Id},{bill.FlatNo},{bill.BillTitle},{bill.Amount},{bill.Month},{bill.CreatedAt}");
                }
                var fileName = "Bills_Report.csv";
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
            }
            else if (type.Equals("visitors"))
            {
                var allvisitors = db.visitors.ToList();
                var filteredVisitors = allvisitors
                    .Where(b => DateTime.TryParse(b.createdAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("ID,Flat No,Purpose,Visitor Name,Visitor Contact,Meet To,Check IN,Check Out,Created At");

                foreach (var visitor in filteredVisitors)
                {
                    sb.AppendLine($"{visitor.id},{visitor.alloted_flats},{visitor.purpose},{visitor.visitor_name},{visitor.visitor_contact},{visitor.meet_to},{visitor.check_in_dt},{visitor.check_out_dt},{visitor.createdAt}");
                }

                var fileName = "Visitors_Report.csv";
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
            }
            else if (type.Equals("complaints"))
            {
                var allcomplaints = db.Complaints.ToList();
                var filteredComplaints = allcomplaints
                    .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("ID,User,Flat No,Complaint,Status,Comments,Created At");

                foreach (var c in filteredComplaints)
                {
                    sb.AppendLine($"{c.Id},{c.User_Email},{c.AllotedFlat},{c.Description},{c.Status},{c.comments},{c.CreatedAt}");
                }

                var fileName = "Complaints_Report.csv";
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
            }
            return BadRequest("Invalid type or data.");
        }

        public IActionResult UserReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserReport(string type, DateTime startdate, DateTime enddate)
        {
            var report = new Report();

            if (!string.IsNullOrEmpty(type))
            {
                string email = HttpContext.Session.GetString("UserEmail");
                if (type.Equals("bills"))
                {
                    var allBills = db.bills.Where(a=> a.user_email.Equals(email)).ToList();
                    var filteredBills = allBills
                        .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                    createdDate >= startdate && createdDate <= enddate)
                        .ToList();
                    report.Bills = filteredBills;
                }

                else if (type.Equals("visitors"))
                {
                    
                    string allotment = db.Flat_Alltmnt.Where(f => f.AllottedTo.Equals(email)).Select(a => a.FlatNo).SingleOrDefault();
                        var allVisitors = db.visitors.Where(v => v.alloted_flats.ToString() == allotment).ToList();
                    var filteredVisitors = allVisitors
                        .Where(b => DateTime.TryParse(b.createdAt, out DateTime createdDate) &&
                                    createdDate >= startdate && createdDate <= enddate)
                        .ToList();
                    report.Visitors = filteredVisitors;
                }

                else if (type.Equals("complaints"))
                {
                    var allComplaints = db.Complaints.Where(a => a.User_Email.Equals(email)).ToList();
                    var filteredComplaints = allComplaints
                        .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                    createdDate >= startdate && createdDate <= enddate)
                        .ToList();
                    report.Complaints = filteredComplaints;
                }


                ViewData["StartDate"] = startdate.ToString("yyyy-MM-dd");
                ViewData["EndDate"] = enddate.ToString("yyyy-MM-dd");


                return View(report);
            }

            return View();
        }

        public IActionResult UserExportToCsv(string type, DateTime startdate, DateTime enddate)
        {
            string email = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(type) && type.Equals("bills"))
            {
                
                var allBills = db.bills.Where(a => a.user_email.Equals(email)).ToList();
                var filteredBills = allBills
                    .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("ID,Flat No,Bill Type,Amount,Month,Created At");

                foreach (var bill in filteredBills)
                {
                    sb.AppendLine($"{bill.Id},{bill.FlatNo},{bill.BillTitle},{bill.Amount},{bill.Month},{bill.CreatedAt}");
                }
                var fileName = "Bills_Report.csv";
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
            }
            else if (type.Equals("visitors"))
            {
                string allotment = db.Flat_Alltmnt.Where(f => f.AllottedTo.Equals(email)).Select(a => a.FlatNo).SingleOrDefault();
                var allVisitors = db.visitors.Where(v => v.alloted_flats.ToString() == allotment).ToList();
                var filteredVisitors = allVisitors
                    .Where(b => DateTime.TryParse(b.createdAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("ID,Flat No,Purpose,Visitor Name,Visitor Contact,Meet To,Check IN,Check Out,Created At");

                foreach (var visitor in filteredVisitors)
                {
                    sb.AppendLine($"{visitor.id},{visitor.alloted_flats},{visitor.purpose},{visitor.visitor_name},{visitor.visitor_contact},{visitor.meet_to},{visitor.check_in_dt},{visitor.check_out_dt},{visitor.createdAt}");
                }

                var fileName = "Visitors_Report.csv";
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
            }
            else if (type.Equals("complaints"))
            {
                var allcomplaints = db.Complaints.Where(a => a.User_Email.Equals(email)).ToList();
                var filteredComplaints = allcomplaints
                    .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("ID,User,Flat No,Complaint,Status,Comments,Created At");

                foreach (var c in filteredComplaints)
                {
                    sb.AppendLine($"{c.Id},{c.User_Email},{c.AllotedFlat},{c.Description},{c.Status},{c.comments},{c.CreatedAt}");
                }

                var fileName = "Complaints_Report.csv";
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
            }
            return BadRequest("Invalid type or data.");
        }

        public IActionResult UserSendReportEmail(string type, DateTime startdate, DateTime enddate)
        {
            string email = HttpContext.Session.GetString("UserEmail");
            StringBuilder sb = new StringBuilder();
            string fileName;
            string subject;
            string body = "Please find the attached report.";

            if (!string.IsNullOrEmpty(type) && type.Equals("bills"))
            {
                var allBills = db.bills.Where(a => a.user_email.Equals(email)).ToList();
                var filteredBills = allBills
                    .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();

                sb.AppendLine("ID,Flat No,Bill Type,Amount,Month,Created At");

                foreach (var bill in filteredBills)
                {
                    sb.AppendLine($"{bill.Id},{bill.FlatNo},{bill.BillTitle},{bill.Amount},{bill.Month},{bill.CreatedAt}");
                }

                fileName = "Bills_Report.csv";
                subject = $"Here Is Your Bills Report between This Date {startdate} to this Date {enddate} ";
            }
            else if (type.Equals("visitors"))
            {
                string allotment = db.Flat_Alltmnt.Where(f => f.AllottedTo.Equals(email)).Select(a => a.FlatNo).SingleOrDefault();
                var allVisitors = db.visitors.Where(v => v.alloted_flats.ToString() == allotment).ToList();
                var filteredVisitors = allVisitors
                    .Where(b => DateTime.TryParse(b.createdAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();

                sb.AppendLine("ID,Flat No,Purpose,Visitor Name,Visitor Contact,Meet To,Check IN,Check Out,Created At");

                foreach (var visitor in filteredVisitors)
                {
                    sb.AppendLine($"{visitor.id},{visitor.alloted_flats},{visitor.purpose},{visitor.visitor_name},{visitor.visitor_contact},{visitor.meet_to},{visitor.check_in_dt},{visitor.check_out_dt},{visitor.createdAt}");
                }

                fileName = "Visitors_Report.csv";
                subject = "Here Is Your Visitors Report between This Date {startdate} to this Date {enddate}";
            }
            else if (type.Equals("complaints"))
            {
                var allcomplaints = db.Complaints.Where(a => a.User_Email.Equals(email)).ToList();
                var filteredComplaints = allcomplaints
                    .Where(b => DateTime.TryParse(b.CreatedAt, out DateTime createdDate) &&
                                createdDate >= startdate && createdDate <= enddate)
                    .ToList();

                sb.AppendLine("ID,User,Flat No,Complaint,Status,Comments,Created At");

                foreach (var c in filteredComplaints)
                {
                    sb.AppendLine($"{c.Id},{c.User_Email},{c.AllotedFlat},{c.Description},{c.Status},{c.comments},{c.CreatedAt}");
                }

                fileName = "Complaints_Report.csv";
                subject = "Here Is Your Complaint Report between This Date {startdate} to this Date {enddate}";
            }
            else
            {
                return BadRequest("Invalid type or data.");
            }


            var csvContent = Encoding.UTF8.GetBytes(sb.ToString());
            using (var memoryStream = new MemoryStream(csvContent))
            {
                var attachment = new Attachment(memoryStream, fileName, "text/csv");
                var mail = new MailMessage();

                mail.From = new MailAddress("mpoke1928@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Society Connect - Invoice";
                mail.Body = "Dear User,\n\nAttached is your invoice. Please review it.\n\nBest Regards,\nSociety Connect";

                mail.Attachments.Add(attachment);

                var smtp = new SmtpClient("smtp.gmail.com", 587);

                smtp.Credentials = new NetworkCredential("mpoke1928@gmail.com", "dhfx widi jvps itnr");
                smtp.EnableSsl = true;
                smtp.Send(mail);


            }
            TempData["sucess"] = "Report Sends To Email!";
            return RedirectToAction("UserReport");
        }

    }
}

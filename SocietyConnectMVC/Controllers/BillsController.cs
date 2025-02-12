using Microsoft.AspNetCore.Mvc;
using SocietyConnectMVC.Data;
using SocietyConnectMVC.Models;
using System.Reflection.Metadata;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Net.Mail;
using System.Net;
using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc.Razor;
using Razorpay.Api;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Org.BouncyCastle.Utilities.Test.FixedSecureRandom;


namespace SocietyConnectMVC.Controllers
{
    public class BillsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        public BillsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        public IActionResult Index()
        {
            var data = db.bills.ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (search != null)
            {
                var data = db.bills.Where(a => a.BillTitle.Contains(search) || a.FlatNo.Contains(search) || a.Month.Contains(search) || a.Pymnt_type.Contains(search) || a.user_email.Contains(search) || a.Paid_Amount.Contains(search)).ToList();
                return View(data);
            }
            else 
            {
                var data = db.bills.ToList();
                return View(data);
            }
        }

        public IActionResult AddBill()
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
        public IActionResult AddBill(Bill b)
        {
            string email = db.Flat_Alltmnt.Where(x=> x.FlatNo.Equals(b.FlatNo)).Select(a=> a.AllottedTo).SingleOrDefault();


            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content", "bills", $"{b.BillTitle}_{b.Month}.pdf");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                PdfWriter writer = new PdfWriter(fileStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                var document = new iText.Layout.Document(pdfDocument);


                document.Add(new Paragraph("Society Connect")
                .SetFontSize(24)
                //.SetBold()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.BLUE));

                document.Add(new Paragraph($"Monthly Bill - {b.Month}")
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(30));

                // Add date
                document.Add(new Paragraph($"Date: {DateTime.Now:MMMM dd, yyyy}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(12));

                // Add customer information
                document.Add(new Paragraph($"Dear {email},")
                    .SetFontSize(14)
                    //.SetBold()
                    .SetMarginTop(20));


                // Bill summary
                document.Add(new Paragraph("Bill Summary:")
                    .SetFontSize(14)
                    //.SetBold()
                    .SetMarginTop(20));

                document.Add(new Paragraph($"Description: {b.BillTitle}")
                    .SetFontSize(12));

                document.Add(new Paragraph($"Amount: {b.Amount:C}")
                    .SetFontSize(12));

                // Total amount due
                document.Add(new Paragraph($"Total Amount Due: {b.Amount:C}")
                    .SetFontSize(14)
                    //.SetBold()
                    .SetMarginTop(10)
                    .SetFontColor(ColorConstants.RED));

                // Add footer
                document.Add(new LineSeparator(new SolidLine()).SetMarginTop(20).SetMarginBottom(10));

                document.Add(new Paragraph("Best Regards,")
                    .SetFontSize(12));

                document.Add(new Paragraph("Society Connect HR Department")
                    .SetFontSize(12)
                    /*.SetBold()*/);

                document.Add(new Paragraph("Please make the payment by the due date to avoid late charges.")
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(30)
                    .SetFontColor(ColorConstants.GRAY));

                // Close document
                document.Close();
            }
            b = new Bill
            {
                BillTitle = b.BillTitle,
                FlatNo = b.FlatNo,
                Amount = b.Amount,
                Month = b.Month,
                CreatedAt = DateTime.Now.ToString("dd-MM-yyyy"),
                BillPdf = $"Content/bills/{b.BillTitle}_{b.Month}.pdf",
                user_email = email,
                Paid_Amount = "Not Paid",
                Pymnt_type = null

            };
            db.Add(b);
            db.SaveChanges();
            Notification n = new Notification();
            n.Email = email;
            n.Message = $"A new bill has been added for your flat {b.FlatNo}.";
            n.Url = $"/Bills/ViewUserBill/{b.Id}";
            db.notifications.Add(n);
            db.SaveChanges();
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("mpoke1928@gmail.com");
            mail.Subject = "Your Monthly Bill from Society Connect";
            mail.Body = $"Hello User,\n\nAttached is your monthly bill for {b.BillTitle}. Please review it and make the payment by the due date .\n\nBest Regards,\nSociety Connect";

            mail.Attachments.Add(new Attachment(filePath));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("mpoke1928@gmail.com", "dhfx widi jvps itnr");
            smtp.EnableSsl = true;

            smtp.Send(mail);
            TempData["sucess"] = "Bill Genrated Successfully!!";
            return RedirectToAction("Index");
        }

        public IActionResult ViewBill(int id)
        {
            var data = db.bills.Find(id);
            return View(data);
        }

        public IActionResult EditBill(int id)
        {
            var data = db.bills.Find(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult EditBill(Bill b)
        {
            var data = db.bills.Find(b.Id);
            if (data != null)
            {
                
                data.BillTitle = b.BillTitle;
                data.FlatNo = b.FlatNo;
                data.Amount = b.Amount;
                data.Month = b.Month;

                db.bills.Update(data);
                db.SaveChanges();
            }
            TempData["update"] = "Bill Updated Successfully!!";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteBill(int id)
        {
            var data = db.bills.Find(id);
            db.bills.Remove(data);
            db.SaveChanges();
            TempData["error"] = "Bill Deleted Successfully!!";
            return RedirectToAction("Index");
        }

        public IActionResult UserBills()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var data = db.bills.Where(a => a.user_email.Equals(userEmail)).ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult UserBills(string search)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
       
            if (search != null)
            {
                var data = db.bills.Where(a => a.user_email.Equals(userEmail)).ToList();
                var filterData = data.Where(a => a.BillTitle.Contains(search) || a.FlatNo.Contains(search) || a.Month.Contains(search) || a.Pymnt_type.Contains(search) || a.user_email.Contains(search) || a.Paid_Amount.Contains(search)).ToList();
                return View(filterData);
            }
            else
            {
                var data = db.bills.Where(a => a.user_email.Equals(userEmail)).ToList();
                return View(data);
            }
        }

        public IActionResult ViewUserBill(int id)
        {
            var data = db.bills.Find(id);
            return View(data);
        }


        public IActionResult PayBill(Bill b)
        {
            
            var data = db.bills.FirstOrDefault(bill => bill.Id == b.Id);

            if (data != null && data.Paid_Amount == "Not Paid")
            { 

                if (b.Pymnt_type == "Cash")
                {
                    
                    GenerateBill(data.user_email, data.BillTitle, data.Amount.ToString(), data.Month);
                    data.Paid_Amount = data.Amount.ToString();
                    data.Pymnt_type = "Cash";

                    db.SaveChanges();
                    Notification n = new Notification();
                    n.Email = "admin@gmail.com";
                    n.Message = $"Bill Payment Recieved From Flat No {data.FlatNo}";
                    n.Url = $"/Bills/ViewBill/{data.Id}";
                    db.notifications.Add(n);
                    db.SaveChanges();
                    
                }
                else if (b.Pymnt_type == "Net Banking")
                {
                    // Handle Razorpay integration
                    double amount = double.Parse(data.Amount.ToString());
                    string keyId = "rzp_test_Kl7588Yie2yJTV";
                    string keySecret = "6dN9Nqs7M6HPFMlL45AhaTgp";

                    RazorpayClient razorpayClient = new RazorpayClient(keyId, keySecret);

                    // Create Razorpay order
                    var options = new Dictionary<string, object>
                    {
                        { "amount", amount * 100 }, // Convert to paisa
                        { "currency", "INR" },
                        { "receipt", $"receipt_{data.Id}" },
                        { "payment_capture", 1 } // Auto-capture payment
                    };

                    Razorpay.Api.Order order = razorpayClient.Order.Create(options);

                    // Pass Razorpay details to the view
                    ViewBag.RazorpayKey = keyId;
                    ViewBag.bill_id = data.Id;
                    ViewBag.OrderId = order["id"].ToString();
                    ViewBag.Amount = amount * 100;
                    ViewBag.Email = b.user_email;
                    ViewBag.Contact = "7208921898"; 

                    return View("RazorpayCheckout");
                }
                else
                {
                    TempData["error"] = "Select a valid payment method.";
                }
            }
            else
            {
                TempData["error"] = "Bill payment failed! The bill may already be paid or does not exist.";
            }
            TempData["sucess"] = "Payment has been recorded successfully & Invoice Sends To Email!";
            return RedirectToAction("UserBills");
        }

        public IActionResult Success(string paymentId , int id)
        {
            if (!string.IsNullOrEmpty(paymentId))
            {
                var b = db.bills.FirstOrDefault(bill => bill.Id == id);
                GenerateBill(b.user_email, b.BillTitle, b.Amount.ToString(), b.Month);
                b.Paid_Amount = b.Amount.ToString();
                b.Pymnt_type = "Net Banking";
                db.SaveChanges();
                Notification n = new Notification();
                n.Email = "admin@gmail.com";
                n.Message = $"Bill Payment Recieved From Flat No {b.FlatNo}";
                n.Url = $"/Bills/ViewBill/{b.Id}";
                db.notifications.Add(n);
                db.SaveChanges();
                TempData["sucess"] = $"Payment successful & Invoice Sends to Email!";
                return RedirectToAction("UserBills");
            }

            TempData["error"] = "Payment verification failed!";
            return RedirectToAction("UserBills");
        }


        public IActionResult GenerateBill(string email, string billTitle, string amount, string month)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content", "Invoices", $"Invoice_{Guid.NewGuid()}.pdf");


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    PdfWriter writer = new PdfWriter(fileStream);
                    PdfDocument pdfDocument = new PdfDocument(writer);
                    var document = new iText.Layout.Document(pdfDocument);

                    document.Add(new Paragraph("Society Connect")
                        .SetFontSize(24)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetMarginBottom(20));

                    document.Add(new Paragraph($"Monthly Bill - {month}")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(30));

                    document.Add(new Paragraph($"Date: {DateTime.Now:MMMM dd, yyyy}")
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFontSize(12)
                        .SetMarginBottom(20));

                    document.Add(new Paragraph($"Dear {email},")
                        .SetFontSize(14)
                        .SetMarginTop(10));

                    Table table = new Table(2).UseAllAvailableWidth();
                    table.AddCell(CreateCell("Bill Title:", true));
                    table.AddCell(CreateCell(billTitle, false));
                    table.AddCell(CreateCell("Month:", true));
                    table.AddCell(CreateCell(month, false));
                    table.AddCell(CreateCell("Amount:", true));
                    table.AddCell(CreateCell($"{amount:C}", false));
                    document.Add(table);

                    document.Add(new Paragraph($"Total Amount Paid: {amount:C}")
                        .SetFontSize(14)
                        .SetMarginTop(20)
                        .SetFontColor(ColorConstants.RED));

                    document.Add(new LineSeparator(new SolidLine()).SetMarginTop(20).SetMarginBottom(10));
                    document.Add(new Paragraph("Best Regards,")
                        .SetFontSize(12));
                    document.Add(new Paragraph("Society Connect HR Department")
                        .SetFontSize(12));
                    document.Add(new Paragraph("Please make the payment by the due date to avoid late charges.")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(30)
                        .SetFontColor(ColorConstants.GRAY));
                    document.Close();
                }


                SendInvoiceEmail(email, filePath);


                var fileName = Path.GetFileName(filePath);
                var fileStream1 = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream1, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return BadRequest("An error occurred while generating the bill.");
            }
        }


        private Cell CreateCell(string content, bool isHeader)
        {
            return new Cell()
                .Add(new Paragraph(content))
                .SetFontSize(isHeader ? 12 : 10)
        
                .SetBackgroundColor(isHeader ? ColorConstants.LIGHT_GRAY : ColorConstants.WHITE)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetPadding(5);
        }

        private void SendInvoiceEmail(string recipientEmail, string filePath)
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("mpoke1928@gmail.com");
                mail.To.Add(recipientEmail);
                mail.Subject = "Society Connect - Invoice";
                mail.Body = "Dear User,\n\nAttached is your invoice. Please review it.\n\nBest Regards,\nSociety Connect";

                mail.Attachments.Add(new Attachment(filePath));

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("mpoke1928@gmail.com", "dhfx widi jvps itnr");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

    }
}

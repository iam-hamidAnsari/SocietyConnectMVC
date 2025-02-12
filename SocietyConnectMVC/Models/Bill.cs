namespace SocietyConnectMVC.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string ?FlatNo { get; set; }
        public string ?user_email { get; set; }
        public string ?BillTitle { get; set; }
        public decimal Amount { get; set; }
        public string ?Month { get; set; }
        public string ?BillPdf { get; set; }
        public string ?CreatedAt { get; set; }
        public string? Paid_Amount { get; set; }
        public string? Pymnt_type { get; set; }
    }
}

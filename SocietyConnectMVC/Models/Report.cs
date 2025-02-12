namespace SocietyConnectMVC.Models
{
    public class Report
    {
        public List<Bill> Bills { get; set; }
        public List<Visitor> Visitors { get; set; }
        public List<Complaint> Complaints { get; set; }

    }
}

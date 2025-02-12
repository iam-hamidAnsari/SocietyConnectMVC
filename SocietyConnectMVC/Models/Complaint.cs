namespace SocietyConnectMVC.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public string? User_Email { get; set; }

        public int? AllotedFlat { get; set; }

        public string? Complaint_Type { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public string? CreatedAt { get; set; }

        public string? ResolvedAt { get; set; }

        public string? comments { get; set; }
    }
}

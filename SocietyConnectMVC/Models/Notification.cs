namespace SocietyConnectMVC.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public int Status { get; set; } = 0;
        
    }
}

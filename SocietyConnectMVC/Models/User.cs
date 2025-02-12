namespace SocietyConnectMVC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Email { get; set; }
        public string ?Password { get; set; }
        public string ?role { get; set; }
        public string ?profile_image { get; set; }
        public string ?CreatedAt { get; set; }

    }
}

using System.Diagnostics.CodeAnalysis;

namespace SocietyConnectMVC.Models
{
    public class Visitor
    {
        public int id { get; set; }

        public int alloted_flats { get; set; }

        public string visitor_name { get; set; }

        public string visitor_contact { get; set; }

        public string meet_to { get; set; }

        public string purpose { get; set; }

        public string check_in_dt { get; set; }
        [AllowNull]
        public string? check_out_dt { get; set; }
        [AllowNull]
        public string? createdAt { get; set; }
    }
}

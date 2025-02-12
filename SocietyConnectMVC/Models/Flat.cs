using System.ComponentModel.DataAnnotations;

namespace SocietyConnectMVC.Models
{
    public class Flat
    {
        [Key]
        public int id { get; set; }


        public string flat_no { get; set; }

        public int floor_no { get; set; }


        public string block_no { get; set; }


        public string flat_type { get; set; }

        public string is_allotted { get; set; }
    }
}

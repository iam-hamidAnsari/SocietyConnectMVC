using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SocietyConnectMVC.Models
{
    public class FlatAlltmnt
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("allotted_to")]
        [StringLength(100)]
        public string AllottedTo { get; set; }

        [Required]
        [Column("flat_no")]
        [StringLength(100)]
        public string FlatNo { get; set; }

        [Required]
        [Column("floor_no")]
        public int FloorNo { get; set; }

        [Required]
        [Column("block_no")]
        [StringLength(100)]
        public string BlockNo { get; set; }

        [Required]
        [Column("flat_type")]
        [StringLength(100)]
        public string FlatType { get; set; }

        [Column("move_in_dt")]
        [DataType(DataType.Date)]
        public string? MoveInDt { get; set; }

        [AllowNull]
        [Column("move_out_dt")]
        [DataType(DataType.Date)]
        public string? MoveOutDt { get; set; }
    }
}

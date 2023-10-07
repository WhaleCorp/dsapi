using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Tables
{
    [Table("Monitors")]
    public class Monitor
    {
        public Monitor(string code)
        {
            Code = code;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [Required,StringLength(100)]
        public string Code { get; set; }
        [StringLength(100)]
        public string? Location { get; set; }
        [StringLength(100)]
        public string? Size { get; set; }
    }
}

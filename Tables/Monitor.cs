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
        public string? Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string? Location { get; set; }
        public string? Size { get; set; }
    }
}

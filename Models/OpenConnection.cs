using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Models
{
    [Table("OpenConnections")]
    public class OpenConnection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Guid { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MonitorId { get; set; }
    }
}

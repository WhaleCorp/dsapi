using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Models
{
    public class MonitorMessage
    {
        private string guid = "";
        private string data = "";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Guid { get => guid; set => guid = value; }
        [Required]
        public string Data { get => data; set => data = value; }
        [Required]
        public int MonitorId { get; set; }
    }
}

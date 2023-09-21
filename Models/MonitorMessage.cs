using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Models
{
    public class MonitorMessage
    {
        [Required]
        public string Data { get; set; }
    }
}

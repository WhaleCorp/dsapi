using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Models
{
    public class MonitorMessage
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Data { get; set; }
        [Required]
        public string RawData { get; set; }
    }
}

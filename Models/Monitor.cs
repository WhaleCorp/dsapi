using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Models
{
    [Table("Monitors")]
    public class Monitor
    {
        private string nameForUserUse="";

        public Monitor(string monitorName)
        {
            MonitorName = monitorName;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NameForUserUse { get => nameForUserUse; set => nameForUserUse = value; }
        [Required]
        public string MonitorName { get; set; }
    }
}

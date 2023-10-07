using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Tables
{
    [Table("MonitorsData")]
    public class MonitorData
    {
        public MonitorData(int userId, string code)
        {
            UserId = userId;
            Code = code;
            Data = "";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required, StringLength(200)]
        public string Code { get; set; }
        public string Data { get; set; }
    }
}

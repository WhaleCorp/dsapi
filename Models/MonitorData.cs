using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Models.MonitorData
{
    [Table("MonitorsIsShowsTime")]
    public class MonitorIsShowsTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MonitorId { get; set; }
        public bool IsShowsTime { get; set; }
    }
    [Table("MonitorsImg")]
    public class MonitorImg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MonitorId { get; set; }
        public string Img { get; set; }
    }
    [Table("MonitorsText")]
    public class MonitorText
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int MonitorId { get; set; }
        public string Text { get; set; }
    }
    public class MonitorData
    {
        public int Id { get; set; }
        public List<string> Imgs { get; set; }
        public List<string> Texts { get; set; }
        public bool IsShowsTime { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dsapi.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,StringLength(100)]
        public string Login { get; set; }
        [Required, StringLength(100)]
        public string FirstName { get; set; }
        [Required,StringLength(100)]
        public string LastName { get; set; }
        [Required, StringLength(100)]
        public string Email { get; set; }
        [Required,StringLength(100)]
        public string PhoneNumber { get; set; }
        [Required]
        public int Role { get; set; }

    }
}

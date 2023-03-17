using System.ComponentModel.DataAnnotations;

namespace dsapi.Models
{
    public class LoginModel
    {
        private string login = "";
        private string password = "";

        [Required]
        public string Login { get => login; set => login = value; }
        [Required]
        public string Password { get => password; set => password = value; }
    }
}

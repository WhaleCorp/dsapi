using dsapi.DBContext;
using dsapi.Models;

namespace dsapi.Services
{
    public class UserService : IUserService
    {
        private object user = "";

        private object User { get => user; set => user = value; }

        public object GetUserDetails()
        {
            return User;
        }

        public bool IsValidUserInformation(LoginModel model)
        {

            if (user != null)
            {
                User = user;
                return true;
            }
            else return false;
        }
    }
}

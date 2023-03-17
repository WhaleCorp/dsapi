using dsapi.Models;

namespace dsapi.Services
{
    public interface IUserService
    {
        bool IsValidUserInformation(LoginModel model);
        object GetUserDetails();
    }
}

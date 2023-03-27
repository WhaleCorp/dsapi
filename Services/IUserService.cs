using dsapi.Models;

namespace dsapi.Services
{
    public interface IUserService
    {
        int IsValidUserInformation(LoginModel model);
    }
}

using dsapi.Models;

namespace dsapi.Services
{
    public interface IUserService
    {
        User GetById(int id);
        int IsValidUserInformation(LoginModel model);
    }
}

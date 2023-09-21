using dsapi.Models;
using dsapi.Tables;

namespace dsapi.Services
{
    public interface IUserService
    {
        User GetById(int id);
        int IsValidUserInformation(LoginModel model);
    }
}

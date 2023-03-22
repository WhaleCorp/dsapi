using dsapi.DBContext;
using dsapi.Models;

namespace dsapi.Services
{
    public class UserService : IUserService
    {
        private dbcontext _db;
        public UserService(dbcontext db)
        {
            _db = db;
        }

        public bool IsValidUserInformation(LoginModel model)
        {
            var user = _db.User.Join(_db.UserPasswords,
                             user => user.Id,
                             pass => pass.UserId,
                             (user, pass) => new { user, pass }
                             ).Where(u => u.user.Login == model.Login && u.pass.Password == model.Password);

            foreach (var row in user)
                if (row != null)
                    return true;
                else return false;
            return false;
        }
    }
}

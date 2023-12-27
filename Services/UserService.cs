using dsapi.DBContext;
using dsapi.Models;
using dsapi.Tables;
using Microsoft.AspNetCore.Identity;

namespace dsapi.Services
{
    public class UserService : IUserService
    {
        private dbcontext _db;
        private PasswordHasher<User> hash = new PasswordHasher<User>();
        public UserService(dbcontext db)
        {
            _db = db;
        }

        public User GetById(int id)
        {
            User user = _db.User.First(u => u.Id == id);
            return user;
        }

        public IdRoleModel IsValidUserInformation(LoginModel model)
        {
            var user = _db.User.Where(u => u.Login == model.Login);

            foreach (var row in user)
                if (row != null && hash.VerifyHashedPassword(row, row.Password, model.Password) == PasswordVerificationResult.Success)
                {
                    string roleName = _db.Role.First(n => n.Id == row.RoleId).RoleName??"";
                    return new IdRoleModel(row.Id, roleName);
                }
                else return new IdRoleModel(00);
            return new IdRoleModel(00);
        }
    }
}

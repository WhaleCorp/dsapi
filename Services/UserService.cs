using dsapi.DBContext;
using dsapi.Models;
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

        public int IsValidUserInformation(LoginModel model)
        {
            var user = _db.User.Where(u => u.Login == model.Login );

            foreach (var row in user)
                if (row != null && hash.VerifyHashedPassword(row, row.Password, model.Password) == PasswordVerificationResult.Success)
                    return row.Id;
                else return 00;
            return 00;
        }
    }
}

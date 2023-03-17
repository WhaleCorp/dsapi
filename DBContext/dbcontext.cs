using dsapi.Models;
using Microsoft.EntityFrameworkCore;

namespace dsapi.DBContext
{
    public class dbcontext:DbContext
    {
        public dbcontext(DbContextOptions options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using dsapi.Tables;

namespace dsapi.DBContext
{
    public class dbcontext : DbContext
    {
        public dbcontext(DbContextOptions options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Tables.Monitor> Monitor { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<MonitorData> MonitorData { get; set; }
    }
}

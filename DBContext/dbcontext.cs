using dsapi.Models;
using Microsoft.EntityFrameworkCore;
using dsapi.Models.MonitorData;

namespace dsapi.DBContext
{
    public class dbcontext : DbContext
    {
        public dbcontext(DbContextOptions options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Models.Monitor> Monitor { get; set; }
        public DbSet<MonitorIsShowsTime> MonitorIsShowsTime { get; set; }
        public DbSet<MonitorImg> MonitorImg { get; set; }
        public DbSet<MonitorText> MonitorText { get; set; }
    }
}

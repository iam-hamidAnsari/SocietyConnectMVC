using Microsoft.EntityFrameworkCore;
using SocietyConnectMVC.Models;

namespace SocietyConnectMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }

        public DbSet<Bill> bills { get; set; }

        public DbSet<Flat> flats { get; set; }

        public DbSet<FlatAlltmnt> Flat_Alltmnt { get; set; }

        public DbSet<Visitor> visitors { get; set; }

        public DbSet<Notification> notifications { get; set; }

        public DbSet<Complaint> Complaints { get; set; }
    }
}

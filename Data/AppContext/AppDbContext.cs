using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Model;

namespace Data.AppContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<EmailPlaceholder> EmailPlaceholders { get; set; }
        //public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<SessionLog> SessionLog { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employee>().HasKey(x => x.EmpID);            
        }
    }
}

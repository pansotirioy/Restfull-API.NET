using Assignment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Service
{
    public class ApplicationDbContext
          : IdentityDbContext<Candidate>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Define your DbSets here
        // public DbSet<YourEntity> YourEntities { get; set; }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CandidatesAnalytics> CandidatesAnalytics { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<SaleCertificate> SaleCertificates { get; set; }
        public DbSet<PhotoId> photoIds { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Department> Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }


    }
}

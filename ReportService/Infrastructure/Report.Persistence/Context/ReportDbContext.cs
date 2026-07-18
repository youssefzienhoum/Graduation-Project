using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities;
using System.Reflection;





namespace Report.Persistence.Context
{
    public class ReportDbContext(DbContextOptions<ReportDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.Report> Reports { get; set; } = null!;
        public DbSet<ReportAttachment> ReportAttachments { get; set; } = null!;
        public DbSet<AiAnalysis> AiAnalyses { get; set; } = null!;
        public DbSet<GPSLocation> GPSLocations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
   }
}
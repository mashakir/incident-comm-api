using Incident.Comm.Integration.Data.Context.Mappings;
using Incident.Comm.Integration.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Incident.Comm.Integration.Data
{
    public class IncidentCommDbContext : DbContext
    {
        public IncidentCommDbContext(DbContextOptions<IncidentCommDbContext> options)
            : base(options)
        {
        }
        public DbSet<IncidentInfo> Incidents { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IncidentInfoMap());
            modelBuilder.ApplyConfiguration(new NotificationMap());
            modelBuilder.ApplyConfiguration(new NotificationTypeMap());
        }
    }
}

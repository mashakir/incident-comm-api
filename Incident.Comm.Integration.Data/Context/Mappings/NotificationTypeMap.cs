using Incident.Comm.Integration.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incident.Comm.Integration.Data.Context.Mappings
{
    public class NotificationTypeMap : IEntityTypeConfiguration<NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            // columns
            builder.Property(c => c.Id);
            builder.Property(c => c.Name)
                .HasColumnType("varchar(20)");
            builder.Property(c => c.Severity)
                .HasColumnType("varchar(10)");
            builder.Property(c => c.Description)
                .HasColumnType("varchar(500)");
        }
    }
}

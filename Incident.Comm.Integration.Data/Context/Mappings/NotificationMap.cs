using Incident.Comm.Integration.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incident.Comm.Integration.Data.Context.Mappings
{
    public class NotificationMap : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // columns
            builder.Property(c => c.Id);

            builder.Property(c => c.Created)
                .HasDefaultValueSql("getutcdate()")
                .IsRequired();

            builder.Property(c => c.Updated);   
            builder.Property(c => c.ImageUrl)
                .HasColumnType("varchar(100)");
            builder.Property(c => c.Description)
                .HasColumnType("varchar(500)"); 

            // relationships
            builder.HasOne(c => c.NotificationType);

            // keys
            builder.HasKey(c => c.Id);
            
        }
    }
}

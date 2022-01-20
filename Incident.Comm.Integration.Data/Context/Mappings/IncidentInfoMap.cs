using Incident.Comm.Integration.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incident.Comm.Integration.Data.Context.Mappings
{
    public class IncidentInfoMap : IEntityTypeConfiguration<IncidentInfo>
    {
        public void Configure(EntityTypeBuilder<IncidentInfo> builder)
        {
            // columns
            builder.Property(c => c.Id);

            builder.Property(c => c.Created)
                .HasDefaultValueSql("getutcdate()")
                .IsRequired();

            builder.Property(c => c.Updated);
            builder.Property(c => c.Description)
                .HasColumnType("varchar(50)");
            builder.Property(c => c.Location)
                .HasColumnType("varchar(10)"); 

            // relationships
            builder.HasMany(c => c.Notifications);

            // keys
            builder.HasKey(c => c.Id);

        }
    }
}

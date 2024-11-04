using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Infrastructure.Data.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("Resources");
        
        builder.HasKey(r => r.Id);

        builder
            .HasMany(r => r.Permissions)
            .WithOne(p => p.Resource)
            .HasForeignKey(p => p.ResourceId)
            .HasPrincipalKey(r => r.Id);
    }
}

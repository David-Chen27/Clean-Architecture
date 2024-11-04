using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Infrastructure.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.Resource)
                .WithMany(r => r.Permissions)
                .HasForeignKey(p => p.ResourceId);

            builder
                .HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId);
        }
    }

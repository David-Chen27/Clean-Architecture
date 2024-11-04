using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Infrastructure.Data.Converters;

namespace Clean_Architecture.Infrastructure.Data.Configurations
{
    public class RoleGroupConfiguration : IEntityTypeConfiguration<RoleGroup>
    {
        public void Configure(EntityTypeBuilder<RoleGroup> builder)
        {
            builder.HasKey(na => na.Id);
            
            builder.Property(u => u.RoleUuid)
                .HasMaxLength(450)
                .IsRequired();
            
            builder.Property(u => u.Uuid)
                .HasConversion(new UlidToStringConverter())
                .HasMaxLength(450);
            
            builder.Property(u => u.Name)
                .HasMaxLength(256)
                .IsRequired();
            
            builder.Property(u => u.Description)
                .HasMaxLength(256);
            
            builder.HasMany(u => u.AccountRoleGroups)
                .WithOne(agrg => agrg.RoleGroup)
                .HasForeignKey(agrg => agrg.RoleGroupId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(u => u.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .HasPrincipalKey(rp => rp.RoleUuid)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

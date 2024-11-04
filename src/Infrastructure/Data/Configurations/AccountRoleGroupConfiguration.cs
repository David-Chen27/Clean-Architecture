using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Infrastructure.Data.Converters;

namespace Clean_Architecture.Infrastructure.Data.Configurations
{
    public class AccountRoleGroupConfiguration : IEntityTypeConfiguration<AccountRoleGroup>
    {
        public void Configure(EntityTypeBuilder<AccountRoleGroup> builder)
        {
            builder.HasKey(na => na.Id);

            builder.Property(u => u.AccountId)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(u => u.RoleGroupId)
                .HasMaxLength(450);

            builder.HasIndex(u => u.AccountId).HasDatabaseName("IX_AccountId");

            builder.HasIndex(u => u.RoleGroupId).HasDatabaseName("IX_RoleGroupId");

            builder.HasOne(arg => arg.Account)
                .WithMany(a => a.AccountRoleGroups)
                .HasForeignKey(arg => arg.AccountId)
                .HasPrincipalKey(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(arg => arg.RoleGroup)
                .WithMany(rg => rg.AccountRoleGroups)
                .HasForeignKey(arg => arg.RoleGroupId)
                .HasPrincipalKey(rg => rg.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

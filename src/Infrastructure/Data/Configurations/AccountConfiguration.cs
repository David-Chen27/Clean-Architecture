using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.ValueObjects;
using Clean_Architecture.Infrastructure.Data.Converters;

namespace Clean_Architecture.Infrastructure.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(na => na.Id);

            builder.Property(u => u.Uuid)
                .HasConversion(new UlidToStringConverter())
                .HasMaxLength(450);

            builder.Property(u => u.UserName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.TelEx)
                .HasMaxLength(50)
                .HasComment("單位");

            builder.Property(u => u.IdNumber)
                .HasMaxLength(50)
                .HasComment("身分證字號");

            builder.Property(u => u.Birthday)
                .HasColumnType("datetime");

            builder.Property(u => u.Contact)
                .HasMaxLength(50)
                .HasComment("聯絡人");

            builder.Property(u => u.Post)
                .HasMaxLength(50)
                .HasComment("聯絡人郵遞區號");

            builder.Property(u => u.CityId)
                .HasMaxLength(26)
                .HasComment("聯絡人縣市");

            builder.Property(u => u.TownId)
                .HasMaxLength(26)
                .HasComment("聯絡人鄉鎮");

            builder.Property(u => u.Address)
                .HasMaxLength(256)
                .HasComment("聯絡人地址");

            builder.Property(u => u.Telephone)
                .HasMaxLength(50)
                .HasComment("聯絡人電話");

            builder.Property(u => u.Mobile)
                .HasMaxLength(50)
                .HasComment("聯絡人手機");

            builder.Property(u => u.Fax)
                .HasMaxLength(50)
                .HasComment("聯絡人傳真");

            builder.Property(u => u.Email)
                .HasMaxLength(256)
                .HasComment("聯絡人電子郵件");

            builder.Property(u => u.EmailConfirmed)
                .IsRequired()
                .HasComment("電子郵件確認");

            builder.Property(u => u.Content)
                .HasMaxLength(256)
                .HasComment("備註");

            builder.Property(u => u.ImageUrl)
                .HasMaxLength(256)
                .HasComment("圖片連結");

            builder.Property(u => u.Order)
                .HasColumnType("int");

            builder.Property(u => u.LoginCount)
                .HasColumnType("int");

            builder.Property(u => u.PasswordThreeTimes)
                .HasComment("前三次密碼");

            builder.Property(u => u.IsDeleted)
                .HasColumnType("int");

            builder.Property(u => u.PasswordUpdateTime)
                .HasColumnType("datetime");

            builder.ComplexProperty(x => x.Sex, cb =>
            {
                cb.Property(x => x.Code)
                    .HasMaxLength(20)
                    .HasColumnName("SexCode")
                    .HasComment("性別");
            });

            builder.HasIndex(u => u.UserName).HasDatabaseName("IX_UserName");

            builder.HasIndex(u => u.Email).HasDatabaseName("IX_Email");

            builder.HasIndex(u => u.Mobile).HasDatabaseName("IX_Mobile");
            
            builder.HasMany(u => u.AccountRoleGroups)
                .WithOne(agrg => agrg.Account)
                .HasForeignKey(agrg => agrg.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

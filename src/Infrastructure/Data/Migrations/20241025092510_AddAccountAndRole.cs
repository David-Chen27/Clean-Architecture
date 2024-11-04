using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean_Architecture.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleGroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TelEx = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "單位"),
                    IdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "身分證字號"),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "聯絡人"),
                    Post = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "聯絡人郵遞區號"),
                    CityId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true, comment: "聯絡人縣市"),
                    TownId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true, comment: "聯絡人鄉鎮"),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "聯絡人地址"),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "聯絡人電話"),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "聯絡人手機"),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "聯絡人傳真"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "聯絡人電子郵件"),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, comment: "電子郵件確認"),
                    Content = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "備註"),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, comment: "圖片連結"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    LoginCount = table.Column<int>(type: "int", nullable: false),
                    PasswordThreeTimes = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "前三次密碼"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    PasswordUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SexCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "性別"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountRoleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", maxLength: 450, nullable: false),
                    RoleGroupId = table.Column<int>(type: "int", maxLength: 450, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoleGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRoleGroups_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRoleGroups_RoleGroups_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalTable: "RoleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountId",
                table: "AccountRoleGroups",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupId",
                table: "AccountRoleGroups",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Email",
                table: "Accounts",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Mobile",
                table: "Accounts",
                column: "Mobile");

            migrationBuilder.CreateIndex(
                name: "IX_UserName",
                table: "Accounts",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRoleGroups");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "RoleGroups");
        }
    }
}

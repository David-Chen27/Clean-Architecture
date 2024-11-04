using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean_Architecture.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRoleGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleUuid",
                table: "RoleGroups",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleUuid",
                table: "RoleGroups");
        }
    }
}

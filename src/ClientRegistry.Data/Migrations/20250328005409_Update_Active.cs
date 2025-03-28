using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientRegistry.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Active : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Clients");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Clients",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Clients");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Clients",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}

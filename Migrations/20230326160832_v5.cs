using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dsapi.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SystemName",
                table: "Monitors",
                newName: "NameForUserUse");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Monitors",
                newName: "MonitorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameForUserUse",
                table: "Monitors",
                newName: "SystemName");

            migrationBuilder.RenameColumn(
                name: "MonitorName",
                table: "Monitors",
                newName: "Name");
        }
    }
}

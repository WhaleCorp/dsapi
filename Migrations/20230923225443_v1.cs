using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dsapi.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Jsx",
                table: "MonitorsData",
                newName: "Data");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "MonitorsData",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "MonitorsData");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "MonitorsData",
                newName: "Jsx");
        }
    }
}

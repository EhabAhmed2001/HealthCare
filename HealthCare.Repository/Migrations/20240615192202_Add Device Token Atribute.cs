using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceTokenAtribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                schema: "Identity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceToken",
                schema: "Identity",
                table: "AspNetUsers");
        }
    }
}

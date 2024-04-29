using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditNotificationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Notification",
                schema: "Identity",
                newName: "Notification",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Notification",
                schema: "dbo",
                newName: "Notification",
                newSchema: "Identity");
        }
    }
}

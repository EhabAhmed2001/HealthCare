using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditHistoryDoctorDatatypeToNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HistoryDoctorId",
                schema: "dbo",
                table: "History",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HistoryDoctorId",
                schema: "dbo",
                table: "History",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

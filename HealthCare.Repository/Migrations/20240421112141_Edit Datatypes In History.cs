using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditDatatypesInHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserData_HearRate",
                schema: "dbo",
                table: "History");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserData_Temperature",
                schema: "dbo",
                table: "History",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserData_Oxygen",
                schema: "dbo",
                table: "History",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "UserData_HeartRate",
                schema: "dbo",
                table: "History",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserData_HeartRate",
                schema: "dbo",
                table: "History");

            migrationBuilder.AlterColumn<int>(
                name: "UserData_Temperature",
                schema: "dbo",
                table: "History",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "UserData_Oxygen",
                schema: "dbo",
                table: "History",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "UserData_HearRate",
                schema: "dbo",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

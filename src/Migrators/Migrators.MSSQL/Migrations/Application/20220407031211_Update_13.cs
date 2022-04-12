using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Update_13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Datasets",
                newName: "ApproveState");

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Datasets",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Datasets");

            migrationBuilder.RenameColumn(
                name: "ApproveState",
                table: "Datasets",
                newName: "State");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Update_12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "DatasetFileConfigs",
                newName: "SheetName");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "DatasetFileConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "DatasetFileConfigs");

            migrationBuilder.RenameColumn(
                name: "SheetName",
                table: "DatasetFileConfigs",
                newName: "FileData");
        }
    }
}

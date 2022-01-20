using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Update_8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "DatasetAPIConfigs",
                newName: "TableName");

            migrationBuilder.AddColumn<string>(
                name: "DataKey",
                table: "DatasetAPIConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataKey",
                table: "DatasetAPIConfigs");

            migrationBuilder.RenameColumn(
                name: "TableName",
                table: "DatasetAPIConfigs",
                newName: "Key");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNameNew",
                table: "Metadatas");

            migrationBuilder.RenameColumn(
                name: "DataNameOld",
                table: "Metadatas",
                newName: "Data");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Metadatas",
                newName: "DataNameOld");

            migrationBuilder.AddColumn<string>(
                name: "DataNameNew",
                table: "Metadatas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

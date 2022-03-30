using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application;

public partial class Update_11 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TableName",
            table: "DatasetFileConfigs");

        migrationBuilder.DropColumn(
            name: "TableName",
            table: "DatasetDBConfigs");

        migrationBuilder.DropColumn(
            name: "TableName",
            table: "DatasetAPIConfigs");

        migrationBuilder.AddColumn<string>(
            name: "TableName",
            table: "Datasets",
            type: "nvarchar(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TableName",
            table: "Datasets");

        migrationBuilder.AddColumn<string>(
            name: "TableName",
            table: "DatasetFileConfigs",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "TableName",
            table: "DatasetDBConfigs",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "TableName",
            table: "DatasetAPIConfigs",
            type: "nvarchar(max)",
            nullable: true);
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application;

public partial class Update_3 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Metadatas");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Datasets",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Metadata",
            table: "Datasets",
            type: "nvarchar(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Metadata",
            table: "Datasets");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Datasets",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.CreateTable(
            name: "Metadatas",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DatasetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsDisplay = table.Column<bool>(type: "bit", nullable: true),
                LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                Tenant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Metadatas", x => x.Id);
                table.ForeignKey(
                    name: "FK_Metadatas_Datasets_DatasetId",
                    column: x => x.DatasetId,
                    principalTable: "Datasets",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Metadatas_DatasetId",
            table: "Metadatas",
            column: "DatasetId");
    }
}

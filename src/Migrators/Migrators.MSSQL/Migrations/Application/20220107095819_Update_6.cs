using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Update_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_Categories_CategoryId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_DataTypes_DataTypeId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_Licenses_LicenseId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_Organizations_OrganizationId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_ProviderTypes_ProviderTypeId",
                table: "Datasets");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_Categories_CategoryId",
                table: "Datasets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_DataTypes_DataTypeId",
                table: "Datasets",
                column: "DataTypeId",
                principalTable: "DataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_Licenses_LicenseId",
                table: "Datasets",
                column: "LicenseId",
                principalTable: "Licenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_Organizations_OrganizationId",
                table: "Datasets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_ProviderTypes_ProviderTypeId",
                table: "Datasets",
                column: "ProviderTypeId",
                principalTable: "ProviderTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_Categories_CategoryId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_DataTypes_DataTypeId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_Licenses_LicenseId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_Organizations_OrganizationId",
                table: "Datasets");

            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_ProviderTypes_ProviderTypeId",
                table: "Datasets");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_Categories_CategoryId",
                table: "Datasets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_DataTypes_DataTypeId",
                table: "Datasets",
                column: "DataTypeId",
                principalTable: "DataTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_Licenses_LicenseId",
                table: "Datasets",
                column: "LicenseId",
                principalTable: "Licenses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_Organizations_OrganizationId",
                table: "Datasets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_ProviderTypes_ProviderTypeId",
                table: "Datasets",
                column: "ProviderTypeId",
                principalTable: "ProviderTypes",
                principalColumn: "Id");
        }
    }
}

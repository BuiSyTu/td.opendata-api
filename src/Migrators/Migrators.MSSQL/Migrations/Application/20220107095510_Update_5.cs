using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application;

public partial class Update_5 : Migration
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

        migrationBuilder.AlterColumn<Guid>(
            name: "ProviderTypeId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AlterColumn<Guid>(
            name: "OrganizationId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AlterColumn<Guid>(
            name: "LicenseId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AlterColumn<Guid>(
            name: "DataTypeId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AlterColumn<Guid>(
            name: "CategoryId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

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

        migrationBuilder.AlterColumn<Guid>(
            name: "ProviderTypeId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "OrganizationId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "LicenseId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "DataTypeId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "CategoryId",
            table: "Datasets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Datasets_Categories_CategoryId",
            table: "Datasets",
            column: "CategoryId",
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Datasets_DataTypes_DataTypeId",
            table: "Datasets",
            column: "DataTypeId",
            principalTable: "DataTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Datasets_Licenses_LicenseId",
            table: "Datasets",
            column: "LicenseId",
            principalTable: "Licenses",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Datasets_Organizations_OrganizationId",
            table: "Datasets",
            column: "OrganizationId",
            principalTable: "Organizations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Datasets_ProviderTypes_ProviderTypeId",
            table: "Datasets",
            column: "ProviderTypeId",
            principalTable: "ProviderTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

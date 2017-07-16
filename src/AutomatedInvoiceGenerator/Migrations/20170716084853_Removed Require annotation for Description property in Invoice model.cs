using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedInvoiceGenerator.Migrations
{
    public partial class RemovedRequireannotationforDescriptionpropertyinInvoicemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Invoices",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

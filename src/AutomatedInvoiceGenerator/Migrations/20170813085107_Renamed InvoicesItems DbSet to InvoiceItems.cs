using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedInvoiceGenerator.Migrations
{
    public partial class RenamedInvoicesItemsDbSettoInvoiceItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesItems_Invoices_InvoiceId",
                table: "InvoicesItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesItems_ServiceItems_ServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesItems_ServiceItems_SubscriptionServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoicesItems",
                table: "InvoicesItems");

            migrationBuilder.RenameTable(
                name: "InvoicesItems",
                newName: "InvoiceItems");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesItems_SubscriptionServiceItemId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_SubscriptionServiceItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesItems_ServiceItemId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_ServiceItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesItems_OneTimeServiceItemId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_OneTimeServiceItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesItems_InvoiceId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoiceItems",
                column: "OneTimeServiceItemId",
                principalTable: "ServiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_ServiceItems_ServiceItemId",
                table: "InvoiceItems",
                column: "ServiceItemId",
                principalTable: "ServiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_ServiceItems_SubscriptionServiceItemId",
                table: "InvoiceItems",
                column: "SubscriptionServiceItemId",
                principalTable: "ServiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_ServiceItems_ServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_ServiceItems_SubscriptionServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems");

            migrationBuilder.RenameTable(
                name: "InvoiceItems",
                newName: "InvoicesItems");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_SubscriptionServiceItemId",
                table: "InvoicesItems",
                newName: "IX_InvoicesItems_SubscriptionServiceItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_ServiceItemId",
                table: "InvoicesItems",
                newName: "IX_InvoicesItems_ServiceItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_OneTimeServiceItemId",
                table: "InvoicesItems",
                newName: "IX_InvoicesItems_OneTimeServiceItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoicesItems",
                newName: "IX_InvoicesItems_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoicesItems",
                table: "InvoicesItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesItems_Invoices_InvoiceId",
                table: "InvoicesItems",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoicesItems",
                column: "OneTimeServiceItemId",
                principalTable: "ServiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesItems_ServiceItems_ServiceItemId",
                table: "InvoicesItems",
                column: "ServiceItemId",
                principalTable: "ServiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesItems_ServiceItems_SubscriptionServiceItemId",
                table: "InvoicesItems",
                column: "SubscriptionServiceItemId",
                principalTable: "ServiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

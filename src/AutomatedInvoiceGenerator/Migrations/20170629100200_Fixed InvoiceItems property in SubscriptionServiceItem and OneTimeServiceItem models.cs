using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedInvoiceGenerator.Migrations
{
    public partial class FixedInvoiceItemspropertyinSubscriptionServiceItemandOneTimeServiceItemmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceItems_InvoicesItems_InvoiceItemsId",
                table: "ServiceItems");

            migrationBuilder.DropIndex(
                name: "IX_ServiceItems_InvoiceItemsId",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "InvoiceItemsId",
                table: "ServiceItems");

            migrationBuilder.AddColumn<int>(
                name: "OneTimeServiceItemId",
                table: "InvoicesItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionServiceItemId",
                table: "InvoicesItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesItems_OneTimeServiceItemId",
                table: "InvoicesItems",
                column: "OneTimeServiceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesItems_SubscriptionServiceItemId",
                table: "InvoicesItems",
                column: "SubscriptionServiceItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoicesItems",
                column: "OneTimeServiceItemId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesItems_ServiceItems_SubscriptionServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoicesItems_OneTimeServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoicesItems_SubscriptionServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropColumn(
                name: "OneTimeServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.DropColumn(
                name: "SubscriptionServiceItemId",
                table: "InvoicesItems");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceItemsId",
                table: "ServiceItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItems_InvoiceItemsId",
                table: "ServiceItems",
                column: "InvoiceItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceItems_InvoicesItems_InvoiceItemsId",
                table: "ServiceItems",
                column: "InvoiceItemsId",
                principalTable: "InvoicesItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

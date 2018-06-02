using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AutomatedInvoiceGenerator.Migrations
{
    public partial class MovedInvoiceItemspropertytocommonServiceItementity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_ServiceItems_SubscriptionServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_OneTimeServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_SubscriptionServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "OneTimeServiceItemId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "SubscriptionServiceItemId",
                table: "InvoiceItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OneTimeServiceItemId",
                table: "InvoiceItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionServiceItemId",
                table: "InvoiceItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_OneTimeServiceItemId",
                table: "InvoiceItems",
                column: "OneTimeServiceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_SubscriptionServiceItemId",
                table: "InvoiceItems",
                column: "SubscriptionServiceItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_ServiceItems_OneTimeServiceItemId",
                table: "InvoiceItems",
                column: "OneTimeServiceItemId",
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
    }
}

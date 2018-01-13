using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AutomatedInvoiceGenerator.Migrations
{
    public partial class MovedNETCoreto20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceItems_ServiceItemsSets_ServiceItemsSetId",
                table: "ServiceItems");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "ServiceItemsSetId",
                table: "ServiceItems",
                newName: "SubscriptionServiceItem_ServiceItemsSetId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceItems_ServiceItemsSetId",
                table: "ServiceItems",
                newName: "IX_ServiceItems_SubscriptionServiceItem_ServiceItemsSetId");

            migrationBuilder.AddColumn<int>(
                name: "ServiceItemsSetId",
                table: "ServiceItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItems_ServiceItemsSetId",
                table: "ServiceItems",
                column: "ServiceItemsSetId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceItems_ServiceItemsSets_ServiceItemsSetId",
                table: "ServiceItems",
                column: "ServiceItemsSetId",
                principalTable: "ServiceItemsSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceItems_ServiceItemsSets_SubscriptionServiceItem_ServiceItemsSetId",
                table: "ServiceItems",
                column: "SubscriptionServiceItem_ServiceItemsSetId",
                principalTable: "ServiceItemsSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceItems_ServiceItemsSets_ServiceItemsSetId",
                table: "ServiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceItems_ServiceItemsSets_SubscriptionServiceItem_ServiceItemsSetId",
                table: "ServiceItems");

            migrationBuilder.DropIndex(
                name: "IX_ServiceItems_ServiceItemsSetId",
                table: "ServiceItems");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ServiceItemsSetId",
                table: "ServiceItems");

            migrationBuilder.RenameColumn(
                name: "SubscriptionServiceItem_ServiceItemsSetId",
                table: "ServiceItems",
                newName: "ServiceItemsSetId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceItems_SubscriptionServiceItem_ServiceItemsSetId",
                table: "ServiceItems",
                newName: "IX_ServiceItems_ServiceItemsSetId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceItems_ServiceItemsSets_ServiceItemsSetId",
                table: "ServiceItems",
                column: "ServiceItemsSetId",
                principalTable: "ServiceItemsSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

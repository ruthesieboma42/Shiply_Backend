using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shiply.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverAndPaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DistanceKm",
                table: "Shipments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Shipments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Shipments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Shipments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DriverId",
                table: "Shipments",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Users_DriverId",
                table: "Shipments",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Users_DriverId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_DriverId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DistanceKm",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Shipments");
        }
    }
}

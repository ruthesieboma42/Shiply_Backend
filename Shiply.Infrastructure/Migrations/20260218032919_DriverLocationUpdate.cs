using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shiply.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DriverLocationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Users_DriverId",
                table: "Shipments");

            migrationBuilder.AddColumn<double>(
                name: "CurrentLatitude",
                table: "Users",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrentLongitude",
                table: "Users",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLocationUpdate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Users_DriverId",
                table: "Shipments",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Users_DriverId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CurrentLatitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentLongitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLocationUpdate",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Users_DriverId",
                table: "Shipments",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shiply.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdditionanewlData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PickupAddress",
                table: "Shipments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverAddress",
                table: "Shipments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Shipments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupAddress",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ReceiverAddress",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shipments");
        }
    }
}

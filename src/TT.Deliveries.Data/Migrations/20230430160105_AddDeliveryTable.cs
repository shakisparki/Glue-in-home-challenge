using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TT.Deliveries.Data.Migrations
{
    public partial class AddDeliveryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    AccessWindow_StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AccessWindow_EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Recipient_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Recipient_Address = table.Column<string>(type: "TEXT", nullable: true),
                    Recipient_Email = table.Column<string>(type: "TEXT", nullable: true),
                    Recipient_PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Order_OrderNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Order_Sender = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Delivery");
        }
    }
}

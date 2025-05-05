using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePortfolio.Deliveries.Infra.Migrations
{
    /// <inheritdoc />
    public partial class DeliveryPostgresDbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    expected_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    date_made = table.Column<DateTime>(type: "timestamp", nullable: true),
                    delivery_status = table.Column<string>(type: "varchar(50)", nullable: false),
                    zip_code = table.Column<string>(type: "varchar(20)", nullable: true),
                    state = table.Column<string>(type: "varchar(50)", nullable: true),
                    city = table.Column<string>(type: "varchar(50)", nullable: true),
                    street_address = table.Column<string>(type: "varchar(100)", nullable: true),
                    number_addres = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery");
        }
    }
}

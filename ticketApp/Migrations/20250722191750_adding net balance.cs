using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class addingnetbalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VirtualProfit",
                table: "Balance",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VirtualProfit",
                table: "Balance");
        }
    }
}

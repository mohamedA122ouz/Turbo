using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class seedingIssueCompanyandclient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "IssueCompanies",
                columns: new[] { "Id", "Balance", "Name" },
                values: new object[] { 1, 0m, "IATA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IssueCompanies",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

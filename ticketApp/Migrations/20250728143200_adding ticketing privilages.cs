using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketApp.Migrations
{
    /// <inheritdoc />
    public partial class addingticketingprivilages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Privileges",
                columns: new[] { "Id", "CanCreateBroker", "CanCreatePayment", "CanCreateTicket", "CanDeleteBroker", "CanDeleteClients", "CanDeleteEmployees", "CanDeletePayments", "CanDeletePrivileges", "CanDeleteSettings", "CanDeleteTicket", "CanEditBroker", "CanEditClients", "CanEditEmployees", "CanEditPayments", "CanEditPrivileges", "CanEditSettings", "CanEditTicket", "CanShowAnalytics", "CanViewBroker", "CanViewClients", "CanViewEmployees", "CanViewPayments", "CanViewPrivileges", "CanViewSettings", "CanViewTicket", "Description", "Name" },
                values: new object[] { 2, true, false, true, true, false, false, false, false, false, true, true, false, false, false, false, false, true, false, true, false, false, false, false, false, true, "Ticketing employee.", "Ticketing" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Privileges",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

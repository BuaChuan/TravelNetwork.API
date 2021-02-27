using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "5ea0270a-a307-4869-ab2f-2a5717ff3804");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8b463670-e2f9-4a5c-8771-9ea9fcfbfd0c", "AQAAAAEAACcQAAAAEJsjUhdVhvZQ46Ih5GwxL+AGn+2ILEMpmdA5sQ9tp9hh5qI6KFwKaEEYrSjMFcGj4A==", "ee7189de-8323-4e35-9d28-ac2672962b53" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "f21e50bc-5c58-4aea-be75-a1513e1922e9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eea4b652-9953-425a-a695-8b4bfdba88e1", "AQAAAAEAACcQAAAAEDqmbzw8C4NLCcct8a05WRBIwiZnGgnrnUxxT1Eaam/F2mnEWqIu/m0x+1JBigzNPA==", "dd48042f-9b7a-4c61-8351-7750938e7fd4" });
        }
    }
}

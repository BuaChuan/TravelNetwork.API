using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class Test1Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "2a312428-74d0-4578-9e5f-0e1e7b7828c8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1249571-5d46-438c-bbff-f7f6973082b8", "AQAAAAEAACcQAAAAEB6Wnw5qjiA9RluYRi2aWaYrwogi3Of4x+MCNjN+9y5AmMFdiS4oI8OiwxCB8+tj2w==", "81435e0a-7708-4c40-95d2-7d8726931ebc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}

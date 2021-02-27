using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class Test5Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "3a47d554-1aa5-47fb-bf9b-74a9c5b4050e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f8cd63f5-ff4b-4204-885e-d790a825bbc2", "AQAAAAEAACcQAAAAEBGRl/njKdTNEwGCepRe54snti+AtNuW1KgS2UP178hhdAxBtRBK0rYpRABAoulQXQ==", "a9d88bd2-766c-46aa-922a-156c4aef1aff" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "26b506bf-666e-4a28-b0f7-c64b720a5ee0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0be15957-e83a-42d3-b12e-ddbcb3029209", "AQAAAAEAACcQAAAAEAQLO/lQdPa4p7EYYL3bmjywWYNR8SyIzvN2xB+Yeiz7wq5ajP7eaqYUlLn6jnAscA==", "292ed286-d7e8-4db3-ae70-c63c760709aa" });
        }
    }
}

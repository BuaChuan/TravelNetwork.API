using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class Test3Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "ab635bdb-2b8d-4b58-b7dd-1acc836ec683");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "725b6532-cefc-4927-99bd-80c7a0b840b2", "AQAAAAEAACcQAAAAEOSGLBlZM9LnL+KlocCW5KesIf/Rv3Vst+C0o6jUuKYlMYKBEKyC1R7Y11cCwuL72Q==", "48bc150f-ba7a-4e23-98e4-01965869bcbc" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegApi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UserRolePopulated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "11a3d48f-e7ff-46e6-a897-9664e603a824", "aaee42b9-aa16-40cb-bee4-1c51d41e3e4a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "11a3d48f-e7ff-46e6-a897-9664e603a824", "aaee42b9-aa16-40cb-bee4-1c51d41e3e4a" });
        }
    }
}

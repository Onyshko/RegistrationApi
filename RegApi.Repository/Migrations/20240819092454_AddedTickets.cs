using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegApi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "11a3d48f-e7ff-46e6-a897-9664e603a824", "aaee42b9-aa16-40cb-bee4-1c51d41e3e4a" });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "11a3d48f-e7ff-46e6-a897-9664e603a824", "16494060-c41a-4343-9128-388ed5e61d77" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "11a3d48f-e7ff-46e6-a897-9664e603a824", "16494060-c41a-4343-9128-388ed5e61d77" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "11a3d48f-e7ff-46e6-a897-9664e603a824", "aaee42b9-aa16-40cb-bee4-1c51d41e3e4a" });
        }
    }
}

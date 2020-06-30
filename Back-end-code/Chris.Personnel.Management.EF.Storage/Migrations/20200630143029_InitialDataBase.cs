using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chris.Personnel.Management.EF.Storage.Migrations
{
    public partial class InitialDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    TrueName = table.Column<string>(maxLength: 100, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    CardId = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 100, nullable: false),
                    IsEnabled = table.Column<int>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedUserId = table.Column<Guid>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CardId", "CreatedTime", "CreatedUserId", "Gender", "IsEnabled", "LastModifiedTime", "LastModifiedUserId", "Name", "Password", "Phone", "TrueName" },
                values: new object[] { new Guid("64188630-7f43-49f2-8fb2-d90745339326"), "140226199401294051", new DateTime(2020, 6, 30, 22, 30, 29, 673, DateTimeKind.Local).AddTicks(9182), new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"), 1, 1, null, null, "Admin", "admin123", "13259769759", "施晓勇" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Name",
                table: "User",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

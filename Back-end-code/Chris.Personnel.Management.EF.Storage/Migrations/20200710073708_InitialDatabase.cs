using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chris.Personnel.Management.EF.Storage.Migrations
{
    public partial class InitialDatabase : Migration
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
                    Salt = table.Column<string>(nullable: true),
                    TrueName = table.Column<string>(maxLength: 100, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    CardId = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 100, nullable: false),
                    IsEnabled = table.Column<int>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: true),
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
                columns: new[] { "Id", "CardId", "CreatedTime", "CreatedUserId", "Gender", "IsEnabled", "LastModifiedTime", "LastModifiedUserId", "Name", "Password", "Phone", "Salt", "TrueName" },
                values: new object[] { new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"), "140226199401294051", new DateTime(2020, 7, 9, 23, 15, 14, 0, DateTimeKind.Unspecified), null, 1, 1, null, null, "Admin", "D0988414729740502483CA8D031EC71EA9DDC0D41537B5995F63F88AE3DDE789", "13259769759", "2f460e1f-0193-4c5d-bed4-64b8a88d7f62", "Admin" });

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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chris.Personnel.Management.EF.Storage.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Memo = table.Column<string>(maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedUserId = table.Column<Guid>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

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
                    RoleId = table.Column<Guid>(nullable: true),
                    CreatedUserId = table.Column<Guid>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedUserId = table.Column<Guid>(nullable: true),
                    LastModifiedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedTime", "CreatedUserId", "IsDeleted", "LastModifiedTime", "LastModifiedUserId", "Memo", "Name" },
                values: new object[] { new Guid("32ec1e12-fe6d-4606-902e-6705beb0afc1"), new DateTime(2020, 7, 9, 23, 15, 14, 0, DateTimeKind.Unspecified), new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"), false, null, null, null, "管理员" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CardId", "CreatedTime", "CreatedUserId", "Gender", "IsEnabled", "LastModifiedTime", "LastModifiedUserId", "Name", "Password", "Phone", "RoleId", "Salt", "TrueName" },
                values: new object[] { new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"), "140226199401294051", new DateTime(2020, 7, 9, 23, 15, 14, 0, DateTimeKind.Unspecified), null, 1, 1, null, null, "Admin", "A945FC6ADD2E0AB35C172BD987CC3FC630C4402677F0661A89CAE68F9DF622FC", "13259769759", new Guid("32ec1e12-fe6d-4606-902e-6705beb0afc1"), "9ad6ead0-3444-455c-a03c-35c1013643af", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Name",
                table: "User",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}

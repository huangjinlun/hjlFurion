using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pear.Database.Migrations.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Remark = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: true),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UniqueName = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false),
                    Remark = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemDataCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Remark = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    HigherId = table.Column<int>(type: "int", nullable: true),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemDataCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemDataCategory_SystemDataCategory_HigherId",
                        column: x => x.HigherId,
                        principalTable: "SystemDataCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeacherName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Nickname = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: true),
                    Photo = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    Synopsis = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    SigninedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SchoolClassId = table.Column<int>(type: "int", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_SchoolClass_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleSecurity",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    SecurityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSecurity", x => new { x.RoleId, x.SecurityId });
                    table.ForeignKey(
                        name: "FK_RoleSecurity_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleSecurity_Security_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "Security",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Remark = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemData_SystemDataCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SystemDataCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassTeacher",
                columns: table => new
                {
                    SchoolClassId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTeacher", x => new { x.SchoolClassId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_ClassTeacher_SchoolClass_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassTeacher_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedTime", "Enabled", "IsDeleted", "Name", "Remark", "UpdatedTime" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2020, 12, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "超级管理员", "拥有所有权限", null },
                    { 2, new DateTimeOffset(new DateTime(2020, 12, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "测试用户", "只有测试权限", null }
                });

            migrationBuilder.InsertData(
                table: "Security",
                columns: new[] { "Id", "CreatedTime", "Enabled", "IsDeleted", "Remark", "UniqueName", "UpdatedTime" },
                values: new object[,]
                {
                    { 21, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "security.service.all", "security.service.all", null },
                    { 20, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "security.service.refresh", "security.service.refresh", null },
                    { 19, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "security.service.give", "security.service.give", null },
                    { 18, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "security.service.list", "security.service.list", null },
                    { 17, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "role.service.give", "role.service.give", null },
                    { 16, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "role.service.delete", "role.service.delete", null },
                    { 15, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "role.service.modify", "role.service.modify", null },
                    { 14, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "role.service.add", "role.service.add", null },
                    { 13, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "role.service.list", "role.service.list", null },
                    { 12, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.securities", "user.service.securities", null },
                    { 11, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.securities.self", "user.service.securities.self", null },
                    { 10, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.roles", "user.service.roles", null },
                    { 9, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.roles.self", "user.service.roles.self", null },
                    { 8, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.change.password", "user.service.change.password", null },
                    { 7, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.add", "user.service.add", null },
                    { 6, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.delete", "user.service.delete", null },
                    { 5, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.modify", "user.service.modify", null },
                    { 4, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.modify.self", "user.service.modify.self", null },
                    { 3, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.list", "user.service.list", null },
                    { 2, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.profile", "user.service.profile", null },
                    { 1, new DateTimeOffset(new DateTime(2020, 12, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "user.service.profile.self", "user.service.profile.self", null }
                });

            migrationBuilder.InsertData(
                table: "SystemDataCategory",
                columns: new[] { "Id", "CreatedTime", "Enabled", "HigherId", "IsDeleted", "Name", "Remark", "Sequence", "UpdatedTime" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2020, 12, 22, 15, 30, 20, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, null, false, "性别", "性别", 0, null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Account", "CreatedTime", "Enabled", "Gender", "IsDeleted", "Nickname", "Password", "Photo", "SigninedTime", "Synopsis", "UpdatedTime" },
                values: new object[] { 1, "admin", new DateTimeOffset(new DateTime(2020, 12, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, 0, false, null, "21232f297a57a5a743894a0e4a801fc3", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null });

            migrationBuilder.InsertData(
                table: "RoleSecurity",
                columns: new[] { "RoleId", "SecurityId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 21 },
                    { 1, 20 },
                    { 1, 19 },
                    { 1, 18 },
                    { 1, 17 },
                    { 1, 16 },
                    { 1, 15 },
                    { 1, 14 },
                    { 1, 13 },
                    { 1, 12 },
                    { 1, 10 },
                    { 1, 9 },
                    { 1, 8 },
                    { 1, 7 },
                    { 1, 6 },
                    { 1, 5 },
                    { 1, 4 },
                    { 1, 3 },
                    { 1, 2 },
                    { 1, 11 }
                });

            migrationBuilder.InsertData(
                table: "SystemData",
                columns: new[] { "Id", "CategoryId", "CreatedTime", "Enabled", "IsDeleted", "Name", "Remark", "Sequence", "UpdatedTime" },
                values: new object[,]
                {
                    { 2, 1, new DateTimeOffset(new DateTime(2020, 12, 22, 15, 30, 20, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "女", "女", 1, null },
                    { 1, 1, new DateTimeOffset(new DateTime(2020, 12, 22, 15, 30, 20, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), true, false, "男", "男", 0, null }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ClassTeacher_TeacherId",
                table: "ClassTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSecurity_SecurityId",
                table: "RoleSecurity",
                column: "SecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_SchoolClassId",
                table: "Student",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemData_CategoryId",
                table: "SystemData",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemDataCategory_HigherId",
                table: "SystemDataCategory",
                column: "HigherId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassTeacher");

            migrationBuilder.DropTable(
                name: "RoleSecurity");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "SystemData");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropTable(
                name: "Security");

            migrationBuilder.DropTable(
                name: "SchoolClass");

            migrationBuilder.DropTable(
                name: "SystemDataCategory");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

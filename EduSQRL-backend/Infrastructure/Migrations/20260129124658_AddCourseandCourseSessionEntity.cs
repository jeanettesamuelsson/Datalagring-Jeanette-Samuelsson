using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseandCourseSessionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Concurrency",
                table: "Roles",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Roles",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "(SYSUTCDATETIME())")
                .Annotation("Relational:DefaultConstraintName", "DF_Role_Created");

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Roles",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "(SYSUTCDATETIME())")
                .Annotation("Relational:DefaultConstraintName", "DF_Role_Modified");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(NEWSEQUENTIALID())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Course_Id"),
                    CourseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Concurrency = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Courses_Created"),
                    Modified = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Courses_Modified")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(NEWSEQUENTIALID())")
                        .Annotation("Relational:DefaultConstraintName", "DF_CourseSession_Id"),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Concurrency = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Course_Sessions_Created"),
                    Modified = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Course_Sessions_Modified")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSessions_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSessions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSessions_CourseId",
                table: "CourseSessions",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseSessions");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropColumn(
                name: "Concurrency",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Roles")
                .Annotation("Relational:DefaultConstraintName", "DF_Role_Created");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Roles")
                .Annotation("Relational:DefaultConstraintName", "DF_Role_Modified");
        }
    }
}

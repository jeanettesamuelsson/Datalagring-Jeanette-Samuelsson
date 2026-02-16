using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSessions_Locations_LocationId",
                table: "CourseSessions");

            migrationBuilder.DropTable(
                name: "ParticipantRoles");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(NEWSEQUENTIALID())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:DefaultConstraintName", "DF_Role_Id");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Participants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(NEWSEQUENTIALID())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Registration_Id"),
                    Concurrency = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Registrations_Created"),
                    Modified = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Registrations_Modified"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_CourseSessions",
                        column: x => x.CourseSessionId,
                        principalTable: "CourseSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrations_Participants",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_RoleId",
                table: "Participants",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_CourseSessionId",
                table: "Registrations",
                column: "CourseSessionId");

            migrationBuilder.CreateIndex(
                name: "UQ_Registrations_Participant_Session",
                table: "Registrations",
                columns: new[] { "ParticipantId", "CourseSessionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSessions_Locations_LocationId",
                table: "CourseSessions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Roles",
                table: "Participants",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSessions_Locations_LocationId",
                table: "CourseSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Roles",
                table: "Participants");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Participants_RoleId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Participants");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(NEWSEQUENTIALID())")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_Role_Id");

            migrationBuilder.CreateTable(
                name: "ParticipantRoles",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantRoles", x => new { x.ParticipantId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ParticipantRoles_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipantRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantRoles_RoleId",
                table: "ParticipantRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSessions_Locations_LocationId",
                table: "CourseSessions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

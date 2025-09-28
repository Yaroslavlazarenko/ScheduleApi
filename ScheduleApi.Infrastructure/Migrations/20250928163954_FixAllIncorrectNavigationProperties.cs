using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleBotApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAllIncorrectNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubjects_Subjects_SubjectId",
                table: "GroupSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSubjects_Teachers_TeacherId",
                table: "GroupSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Groups_GroupId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Semesters_SemesterId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_SemesterId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_SubjectId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_TeacherId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_GroupSubjects_SubjectId",
                table: "GroupSubjects");

            migrationBuilder.DropIndex(
                name: "IX_GroupSubjects_TeacherId",
                table: "GroupSubjects");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "GroupSubjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "GroupSubjects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "GroupSubjects",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "GroupSubjects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SemesterId",
                table: "Schedules",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SubjectId",
                table: "Schedules",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TeacherId",
                table: "Schedules",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubjects_SubjectId",
                table: "GroupSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSubjects_TeacherId",
                table: "GroupSubjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubjects_Subjects_SubjectId",
                table: "GroupSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSubjects_Teachers_TeacherId",
                table: "GroupSubjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Groups_GroupId",
                table: "Schedules",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Semesters_SemesterId",
                table: "Schedules",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleBotApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTeacherSubjectRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId1",
                table: "TeacherSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSubjects_SocialMediaTypeId1",
                table: "TeacherSubjects");

            migrationBuilder.DropColumn(
                name: "SocialMediaTypeId1",
                table: "TeacherSubjects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocialMediaTypeId1",
                table: "TeacherSubjects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_SocialMediaTypeId1",
                table: "TeacherSubjects",
                column: "SocialMediaTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId1",
                table: "TeacherSubjects",
                column: "SocialMediaTypeId1",
                principalTable: "SocialMediaTypes",
                principalColumn: "Id");
        }
    }
}

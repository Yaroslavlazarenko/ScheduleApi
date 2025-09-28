using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleBotApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTeacherSubjectSocialMediaTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "TeacherSubjects"
                SET "SocialMediaTypesId" = "SocialMediaTypeId"
                WHERE "SocialMediaTypesId" IS NULL AND "SocialMediaTypeId" IS NOT NULL;
                """
            );
            
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId",
                table: "TeacherSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSubjects_SocialMediaTypeId",
                table: "TeacherSubjects");

            migrationBuilder.DropColumn(
                name: "SocialMediaTypeId",
                table: "TeacherSubjects");
            
            migrationBuilder.RenameColumn(
                name: "SocialMediaTypesId",
                table: "TeacherSubjects",
                newName: "SocialMediaTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_SocialMediaTypesId",
                table: "TeacherSubjects",
                newName: "IX_TeacherSubjects_SocialMediaTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocialMediaTypeId",
                table: "TeacherSubjects",
                newName: "SocialMediaTypesId");
                
            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_SocialMediaTypeId",
                table: "TeacherSubjects",
                newName: "IX_TeacherSubjects_SocialMediaTypesId");
            
            migrationBuilder.AddColumn<int>(
                name: "SocialMediaTypeId",
                table: "TeacherSubjects",
                type: "integer",
                nullable: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_SocialMediaTypeId",
                table: "TeacherSubjects",
                column: "SocialMediaTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId",
                table: "TeacherSubjects",
                column: "SocialMediaTypeId",
                principalTable: "SocialMediaTypes",
                principalColumn: "Id");
        }
    }
}

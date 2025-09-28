using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleBotApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleOverrides_ApplicationDaysOfWeek_SubstituteDayOfWeek~",
                table: "ScheduleOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleOverrides_Groups_GroupId",
                table: "ScheduleOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleOverrides_OverrideTypes_OverrideTypeId",
                table: "ScheduleOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ApplicationDaysOfWeek_ApplicationDayOfWeekId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Pairs_PairId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectInfos_InfoTypes_InfoTypeId",
                table: "SubjectInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SubjectNames_SubjectNameId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SubjectTypes_SubjectTypeId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherInfos_InfoTypes_InfoTypeId",
                table: "TeacherInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId",
                table: "TeacherSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Regions_RegionId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Teachers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleOverrides_ApplicationDaysOfWeek_SubstituteDayOfWeek~",
                table: "ScheduleOverrides",
                column: "SubstituteDayOfWeekId",
                principalTable: "ApplicationDaysOfWeek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleOverrides_Groups_GroupId",
                table: "ScheduleOverrides",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleOverrides_OverrideTypes_OverrideTypeId",
                table: "ScheduleOverrides",
                column: "OverrideTypeId",
                principalTable: "OverrideTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ApplicationDaysOfWeek_ApplicationDayOfWeekId",
                table: "Schedules",
                column: "ApplicationDayOfWeekId",
                principalTable: "ApplicationDaysOfWeek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Pairs_PairId",
                table: "Schedules",
                column: "PairId",
                principalTable: "Pairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectInfos_InfoTypes_InfoTypeId",
                table: "SubjectInfos",
                column: "InfoTypeId",
                principalTable: "InfoTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SubjectNames_SubjectNameId",
                table: "Subjects",
                column: "SubjectNameId",
                principalTable: "SubjectNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SubjectTypes_SubjectTypeId",
                table: "Subjects",
                column: "SubjectTypeId",
                principalTable: "SubjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherInfos_InfoTypes_InfoTypeId",
                table: "TeacherInfos",
                column: "InfoTypeId",
                principalTable: "InfoTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId",
                table: "TeacherSubjects",
                column: "SocialMediaTypeId",
                principalTable: "SocialMediaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Regions_RegionId",
                table: "Users",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleOverrides_ApplicationDaysOfWeek_SubstituteDayOfWeek~",
                table: "ScheduleOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleOverrides_Groups_GroupId",
                table: "ScheduleOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleOverrides_OverrideTypes_OverrideTypeId",
                table: "ScheduleOverrides");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ApplicationDaysOfWeek_ApplicationDayOfWeekId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Pairs_PairId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectInfos_InfoTypes_InfoTypeId",
                table: "SubjectInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SubjectNames_SubjectNameId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SubjectTypes_SubjectTypeId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherInfos_InfoTypes_InfoTypeId",
                table: "TeacherInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId",
                table: "TeacherSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Regions_RegionId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Teachers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleOverrides_ApplicationDaysOfWeek_SubstituteDayOfWeek~",
                table: "ScheduleOverrides",
                column: "SubstituteDayOfWeekId",
                principalTable: "ApplicationDaysOfWeek",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleOverrides_Groups_GroupId",
                table: "ScheduleOverrides",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleOverrides_OverrideTypes_OverrideTypeId",
                table: "ScheduleOverrides",
                column: "OverrideTypeId",
                principalTable: "OverrideTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ApplicationDaysOfWeek_ApplicationDayOfWeekId",
                table: "Schedules",
                column: "ApplicationDayOfWeekId",
                principalTable: "ApplicationDaysOfWeek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Pairs_PairId",
                table: "Schedules",
                column: "PairId",
                principalTable: "Pairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectInfos_InfoTypes_InfoTypeId",
                table: "SubjectInfos",
                column: "InfoTypeId",
                principalTable: "InfoTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SubjectNames_SubjectNameId",
                table: "Subjects",
                column: "SubjectNameId",
                principalTable: "SubjectNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SubjectTypes_SubjectTypeId",
                table: "Subjects",
                column: "SubjectTypeId",
                principalTable: "SubjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherInfos_InfoTypes_InfoTypeId",
                table: "TeacherInfos",
                column: "InfoTypeId",
                principalTable: "InfoTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId",
                table: "TeacherSubjects",
                column: "SocialMediaTypeId",
                principalTable: "SocialMediaTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Regions_RegionId",
                table: "Users",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

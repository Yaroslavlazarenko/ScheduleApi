using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ScheduleBotApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorScheduleTo3NF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_GroupSubjects_TeacherSubjects_TeacherId_SubjectId", table: "GroupSubjects");
            migrationBuilder.DropForeignKey(name: "FK_Schedule_Groups_GroupId", table: "Schedule");
            migrationBuilder.DropForeignKey(name: "FK_Schedule_Semesters_SemesterId", table: "Schedule");
            migrationBuilder.DropForeignKey(name: "FK_Schedule_Subjects_SubjectId", table: "Schedule");
            migrationBuilder.DropForeignKey(name: "FK_Schedule_Teachers_TeacherId", table: "Schedule");
            migrationBuilder.DropForeignKey(name: "FK_GroupSubjects_Groups_GroupId", table: "GroupSubjects");
            migrationBuilder.DropForeignKey(name: "FK_GroupSubjects_Subjects_SubjectId", table: "GroupSubjects");
            migrationBuilder.DropForeignKey(name: "FK_GroupSubjects_Teachers_TeacherId", table: "GroupSubjects");
            
            migrationBuilder.DropPrimaryKey(name: "PK_TeacherSubjects", table: "TeacherSubjects");

            migrationBuilder.AddColumn<int>(name: "Id", table: "TeacherSubjects", type: "integer", nullable: false, defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            
            migrationBuilder.AddColumn<int>(name: "SocialMediaTypeId", table: "TeacherSubjects", type: "integer", nullable: true);
            
            migrationBuilder.AddPrimaryKey(name: "PK_TeacherSubjects", table: "TeacherSubjects", column: "Id");
            
            migrationBuilder.CreateIndex(name: "IX_TeacherSubjects_TeacherId_SubjectId", table: "TeacherSubjects", columns: new[] { "TeacherId", "SubjectId" }, unique: true);
            migrationBuilder.CreateIndex(name: "IX_TeacherSubjects_SocialMediaTypeId", table: "TeacherSubjects", column: "SocialMediaTypeId");
            
            migrationBuilder.Sql(@"DELETE FROM ""GroupSubjects"";"); // Очищаем, т.к. структура полностью меняется
            migrationBuilder.DropPrimaryKey(name: "PK_GroupSubjects", table: "GroupSubjects");
            migrationBuilder.DropIndex(name: "IX_GroupSubjects_TeacherId_SubjectId", table: "GroupSubjects");
            
            migrationBuilder.AlterColumn<int>(name: "TeacherId", table: "GroupSubjects", type: "integer", nullable: true);
            migrationBuilder.AlterColumn<int>(name: "SubjectId", table: "GroupSubjects", type: "integer", nullable: true);
            
            migrationBuilder.AddColumn<int>(name: "Id", table: "GroupSubjects", type: "integer", nullable: false, defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            migrationBuilder.AddColumn<int>(name: "SemesterId", table: "GroupSubjects", type: "integer", nullable: true); // Временно nullable
            migrationBuilder.AddColumn<int>(name: "TeacherSubjectId", table: "GroupSubjects", type: "integer", nullable: true); // Временно nullable
            
            migrationBuilder.Sql(@"
                INSERT INTO ""GroupSubjects"" (""GroupId"", ""SemesterId"", ""TeacherSubjectId"")
                SELECT DISTINCT s.""GroupId"", s.""SemesterId"", ts.""Id""
                FROM ""Schedule"" s
                INNER JOIN ""TeacherSubjects"" ts ON s.""TeacherId"" = ts.""TeacherId"" AND s.""SubjectId"" = ts.""SubjectId""
                WHERE s.""GroupId"" IS NOT NULL AND s.""SemesterId"" IS NOT NULL AND s.""TeacherId"" IS NOT NULL AND s.""SubjectId"" IS NOT NULL;
            ");
            
            migrationBuilder.AlterColumn<int>(name: "SemesterId", table: "GroupSubjects", type: "integer", nullable: false, oldNullable: true);
            migrationBuilder.AlterColumn<int>(name: "TeacherSubjectId", table: "GroupSubjects", type: "integer", nullable: false, oldNullable: true);
            
            migrationBuilder.DropColumn(name: "TeacherId", table: "GroupSubjects");
            migrationBuilder.DropColumn(name: "SubjectId", table: "GroupSubjects");
            
            migrationBuilder.AddPrimaryKey(name: "PK_GroupSubjects", table: "GroupSubjects", column: "Id");
            migrationBuilder.CreateIndex(name: "IX_GroupSubjects_GroupId_TeacherSubjectId_SemesterId", table: "GroupSubjects", columns: new[] { "GroupId", "TeacherSubjectId", "SemesterId" }, unique: true);
            migrationBuilder.CreateIndex(name: "IX_GroupSubjects_SemesterId", table: "GroupSubjects", column: "SemesterId");
            migrationBuilder.CreateIndex(name: "IX_GroupSubjects_TeacherSubjectId", table: "GroupSubjects", column: "TeacherSubjectId");
            
            migrationBuilder.DropPrimaryKey(name: "PK_Schedule", table: "Schedule");
            migrationBuilder.DropIndex(name: "IX_Schedule_ApplicationDayOfWeekId_PairId_GroupId_IsEvenWeek", table: "Schedule");
            
            migrationBuilder.RenameTable(name: "Schedule", newName: "Schedules");
            
            migrationBuilder.AddColumn<int>(name: "GroupSubjectId", table: "Schedules", type: "integer", nullable: true); // Временно nullable
            
            migrationBuilder.Sql(@"
                UPDATE ""Schedules"" AS sch
                SET ""GroupSubjectId"" = gs.""Id""
                FROM ""GroupSubjects"" AS gs
                JOIN ""TeacherSubjects"" AS ts ON gs.""TeacherSubjectId"" = ts.""Id""
                WHERE sch.""GroupId"" = gs.""GroupId""
                  AND sch.""SemesterId"" = gs.""SemesterId""
                  AND sch.""TeacherId"" = ts.""TeacherId""
                  AND sch.""SubjectId"" = ts.""SubjectId"";
            ");
            
            migrationBuilder.AlterColumn<int>(name: "GroupSubjectId", table: "Schedules", type: "integer", nullable: false, oldNullable: true);
            
            migrationBuilder.DropColumn(name: "GroupId", table: "Schedules");
            migrationBuilder.DropColumn(name: "TeacherId", table: "Schedules");
            migrationBuilder.DropColumn(name: "SubjectId", table: "Schedules");
            migrationBuilder.DropColumn(name: "SemesterId", table: "Schedules");

            migrationBuilder.AddPrimaryKey(name: "PK_Schedules", table: "Schedules", column: "Id");
            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ApplicationDayOfWeekId_PairId_GroupSubjectId_IsEvenWeek", 
                table: "Schedules", 
                columns: new[] { "ApplicationDayOfWeekId", "PairId", "GroupSubjectId", "IsEvenWeek" }, 
                unique: true);
            migrationBuilder.CreateIndex(name: "IX_Schedules_GroupSubjectId", table: "Schedules", column: "GroupSubjectId");
            
            migrationBuilder.AddForeignKey(name: "FK_GroupSubjects_Groups_GroupId", table: "GroupSubjects", column: "GroupId", principalTable: "Groups", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_GroupSubjects_Semesters_SemesterId", table: "GroupSubjects", column: "SemesterId", principalTable: "Semesters", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_GroupSubjects_TeacherSubjects_TeacherSubjectId", table: "GroupSubjects", column: "TeacherSubjectId", principalTable: "TeacherSubjects", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedules_GroupSubjects_GroupSubjectId", table: "Schedules", column: "GroupSubjectId", principalTable: "GroupSubjects", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedules_ApplicationDaysOfWeek_ApplicationDayOfWeekId", table: "Schedules", column: "ApplicationDayOfWeekId", principalTable: "ApplicationDaysOfWeek", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedules_Pairs_PairId", table: "Schedules", column: "PairId", principalTable: "Pairs", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId", table: "TeacherSubjects", column: "SocialMediaTypeId", principalTable: "SocialMediaTypes", principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_GroupSubjects_Groups_GroupId", table: "GroupSubjects");
            migrationBuilder.DropForeignKey(name: "FK_GroupSubjects_Semesters_SemesterId", table: "GroupSubjects");
            migrationBuilder.DropForeignKey(name: "FK_GroupSubjects_TeacherSubjects_TeacherSubjectId", table: "GroupSubjects");
            migrationBuilder.DropForeignKey(name: "FK_Schedules_GroupSubjects_GroupSubjectId", table: "Schedules");
            migrationBuilder.DropForeignKey(name: "FK_Schedules_ApplicationDaysOfWeek_ApplicationDayOfWeekId", table: "Schedules");
            migrationBuilder.DropForeignKey(name: "FK_Schedules_Pairs_PairId", table: "Schedules");
            migrationBuilder.DropForeignKey(name: "FK_TeacherSubjects_SocialMediaTypes_SocialMediaTypeId", table: "TeacherSubjects");

            migrationBuilder.DropPrimaryKey(name: "PK_Schedules", table: "Schedules");
            migrationBuilder.DropIndex(name: "IX_Schedules_ApplicationDayOfWeekId_PairId_GroupSubjectId_IsEvenWeek", table: "Schedules");
            migrationBuilder.DropIndex(name: "IX_Schedules_GroupSubjectId", table: "Schedules");
            
            migrationBuilder.AddColumn<int>(name: "GroupId", table: "Schedules", type: "integer", nullable: true);
            migrationBuilder.AddColumn<int>(name: "TeacherId", table: "Schedules", type: "integer", nullable: true);
            migrationBuilder.AddColumn<int>(name: "SubjectId", table: "Schedules", type: "integer", nullable: true);
            migrationBuilder.AddColumn<int>(name: "SemesterId", table: "Schedules", type: "integer", nullable: true);
            
            migrationBuilder.Sql(@"
                UPDATE ""Schedules"" AS sch
                SET ""GroupId"" = gs.""GroupId"", 
                    ""SemesterId"" = gs.""SemesterId"", 
                    ""TeacherId"" = ts.""TeacherId"", 
                    ""SubjectId"" = ts.""SubjectId""
                FROM ""GroupSubjects"" AS gs
                JOIN ""TeacherSubjects"" AS ts ON gs.""TeacherSubjectId"" = ts.""Id""
                WHERE sch.""GroupSubjectId"" = gs.""Id"";
            ");

            migrationBuilder.AlterColumn<int>(name: "GroupId", table: "Schedules", type: "integer", nullable: false, oldNullable: true);
            migrationBuilder.AlterColumn<int>(name: "TeacherId", table: "Schedules", type: "integer", nullable: false, oldNullable: true);
            migrationBuilder.AlterColumn<int>(name: "SubjectId", table: "Schedules", type: "integer", nullable: false, oldNullable: true);
            migrationBuilder.AlterColumn<int>(name: "SemesterId", table: "Schedules", type: "integer", nullable: false, oldNullable: true);
            
            migrationBuilder.DropColumn(name: "GroupSubjectId", table: "Schedules");
            
            migrationBuilder.RenameTable(name: "Schedules", newName: "Schedule");
            
            migrationBuilder.AddPrimaryKey(name: "PK_Schedule", table: "Schedule", column: "Id");
            migrationBuilder.CreateIndex(name: "IX_Schedule_ApplicationDayOfWeekId_PairId_GroupId_IsEvenWeek", table: "Schedule", columns: new[] { "ApplicationDayOfWeekId", "PairId", "GroupId", "IsEvenWeek" }, unique: true);
            
            migrationBuilder.DropPrimaryKey(name: "PK_GroupSubjects", table: "GroupSubjects");
            migrationBuilder.DropIndex(name: "IX_GroupSubjects_GroupId_TeacherSubjectId_SemesterId", table: "GroupSubjects");
            migrationBuilder.DropIndex(name: "IX_GroupSubjects_SemesterId", table: "GroupSubjects");
            migrationBuilder.DropIndex(name: "IX_GroupSubjects_TeacherSubjectId", table: "GroupSubjects");

            migrationBuilder.DropColumn(name: "Id", table: "GroupSubjects");
            migrationBuilder.DropColumn(name: "SemesterId", table: "GroupSubjects");
            migrationBuilder.DropColumn(name: "TeacherSubjectId", table: "GroupSubjects");
            
            migrationBuilder.AddColumn<int>(name: "TeacherId", table: "GroupSubjects", type: "integer", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "SubjectId", table: "GroupSubjects", type: "integer", nullable: false, defaultValue: 0);

            migrationBuilder.AddPrimaryKey(name: "PK_GroupSubjects", table: "GroupSubjects", columns: new[] { "GroupId", "TeacherId", "SubjectId" });
            migrationBuilder.CreateIndex(name: "IX_GroupSubjects_TeacherId_SubjectId", table: "GroupSubjects", columns: new[] { "TeacherId", "SubjectId" });
            
            migrationBuilder.DropPrimaryKey(name: "PK_TeacherSubjects", table: "TeacherSubjects");
            migrationBuilder.DropIndex(name: "IX_TeacherSubjects_TeacherId_SubjectId", table: "TeacherSubjects");
            migrationBuilder.DropIndex(name: "IX_TeacherSubjects_SocialMediaTypeId", table: "TeacherSubjects");
            
            migrationBuilder.DropColumn(name: "Id", table: "TeacherSubjects");
            migrationBuilder.DropColumn(name: "SocialMediaTypeId", table: "TeacherSubjects");
            
            migrationBuilder.AddPrimaryKey(name: "PK_TeacherSubjects", table: "TeacherSubjects", columns: new[] { "TeacherId", "SubjectId" });
            
            migrationBuilder.AddForeignKey(name: "FK_GroupSubjects_Groups_GroupId", table: "GroupSubjects", column: "GroupId", principalTable: "Groups", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_GroupSubjects_Subjects_SubjectId", table: "GroupSubjects", column: "SubjectId", principalTable: "Subjects", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_GroupSubjects_Teachers_TeacherId", table: "GroupSubjects", column: "TeacherId", principalTable: "Teachers", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_GroupSubjects_TeacherSubjects_TeacherId_SubjectId", table: "GroupSubjects", columns: new[] { "TeacherId", "SubjectId" }, principalTable: "TeacherSubjects", principalColumns: new[] { "TeacherId", "SubjectId" }, onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.AddForeignKey(name: "FK_Schedule_ApplicationDaysOfWeek_ApplicationDayOfWeekId", table: "Schedule", column: "ApplicationDayOfWeekId", principalTable: "ApplicationDaysOfWeek", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedule_Groups_GroupId", table: "Schedule", column: "GroupId", principalTable: "Groups", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedule_Pairs_PairId", table: "Schedule", column: "PairId", principalTable: "Pairs", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedule_Semesters_SemesterId", table: "Schedule", column: "SemesterId", principalTable: "Semesters", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedule_Subjects_SubjectId", table: "Schedule", column: "SubjectId", principalTable: "Subjects", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Schedule_Teachers_TeacherId", table: "Schedule", column: "TeacherId", principalTable: "Teachers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }
    }
}
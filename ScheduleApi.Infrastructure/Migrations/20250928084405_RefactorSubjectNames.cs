using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ScheduleBotApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSubjectNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectNames", x => x.Id);
                });
            
            migrationBuilder.AddColumn<int>(
                name: "SubjectNameId",
                table: "Subjects",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(@"
                INSERT INTO ""SubjectNames"" (""FullName"", ""ShortName"", ""Abbreviation"")
                SELECT DISTINCT ""Name"", ""ShortName"", ""Abbreviation"" FROM ""Subjects""
            ");
            
            migrationBuilder.Sql(@"
                UPDATE ""Subjects"" AS s
                SET ""SubjectNameId"" = sn.""Id""
                FROM ""SubjectNames"" AS sn
                WHERE s.""Name"" = sn.""FullName""
                  AND s.""ShortName"" = sn.""ShortName""
                  AND s.""Abbreviation"" = sn.""Abbreviation"";
            ");
            
            migrationBuilder.AlterColumn<int>(
                name: "SubjectNameId",
                table: "Subjects",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
            
            migrationBuilder.DropIndex(
                name: "IX_Subjects_Name_SubjectTypeId",
                table: "Subjects");
            
            migrationBuilder.DropColumn(name: "Name", table: "Subjects");
            migrationBuilder.DropColumn(name: "ShortName", table: "Subjects");
            migrationBuilder.DropColumn(name: "Abbreviation", table: "Subjects");
            
            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectNameId_SubjectTypeId",
                table: "Subjects",
                columns: new[] { "SubjectNameId", "SubjectTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNames_Abbreviation",
                table: "SubjectNames",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SubjectNames_SubjectNameId",
                table: "Subjects",
                column: "SubjectNameId",
                principalTable: "SubjectNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "Name", table: "Subjects", type: "text", nullable: true);
            migrationBuilder.AddColumn<string>(name: "ShortName", table: "Subjects", type: "text", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Abbreviation", table: "Subjects", type: "text", nullable: true);
            
            migrationBuilder.Sql(@"
                UPDATE ""Subjects"" AS s
                SET ""Name"" = sn.""FullName"",
                    ""ShortName"" = sn.""ShortName"",
                    ""Abbreviation"" = sn.""Abbreviation""
                FROM ""SubjectNames"" AS sn
                WHERE s.""SubjectNameId"" = sn.""Id"";
            ");
            
            migrationBuilder.AlterColumn<string>(name: "Name", table: "Subjects", type: "text", nullable: false, oldClrType: typeof(string), oldType: "text", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "ShortName", table: "Subjects", type: "text", nullable: false, oldClrType: typeof(string), oldType: "text", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "Abbreviation", table: "Subjects", type: "text", nullable: false, oldClrType: typeof(string), oldType: "text", oldNullable: true);
            
            migrationBuilder.DropForeignKey(name: "FK_Subjects_SubjectNames_SubjectNameId", table: "Subjects");
            migrationBuilder.DropIndex(name: "IX_Subjects_SubjectNameId_SubjectTypeId", table: "Subjects");
            migrationBuilder.DropIndex(name: "IX_SubjectNames_Abbreviation", table: "SubjectNames");
            
            migrationBuilder.DropColumn(name: "SubjectNameId", table: "Subjects");
            
            migrationBuilder.DropTable(name: "SubjectNames");
            
            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name_SubjectTypeId",
                table: "Subjects",
                columns: new[] { "Name", "SubjectTypeId" },
                unique: true);
        }
    }
}

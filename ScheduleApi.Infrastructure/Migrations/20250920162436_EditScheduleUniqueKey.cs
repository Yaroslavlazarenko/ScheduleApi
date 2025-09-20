using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleBotApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditScheduleUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_ApplicationDayOfWeekId_PairId_GroupId",
                table: "Schedule");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ApplicationDayOfWeekId_PairId_GroupId_IsEvenWeek",
                table: "Schedule",
                columns: new[] { "ApplicationDayOfWeekId", "PairId", "GroupId", "IsEvenWeek" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedule_ApplicationDayOfWeekId_PairId_GroupId_IsEvenWeek",
                table: "Schedule");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ApplicationDayOfWeekId_PairId_GroupId",
                table: "Schedule",
                columns: new[] { "ApplicationDayOfWeekId", "PairId", "GroupId" },
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace KooliProjekt.Migrations
{
    public partial class SongSchedule3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongSchedule_Schedules_ScheduleId",
                table: "SongSchedule");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "SongSchedule",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SongSchedule_Schedules_ScheduleId",
                table: "SongSchedule",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongSchedule_Schedules_ScheduleId",
                table: "SongSchedule");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "SongSchedule",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_SongSchedule_Schedules_ScheduleId",
                table: "SongSchedule",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

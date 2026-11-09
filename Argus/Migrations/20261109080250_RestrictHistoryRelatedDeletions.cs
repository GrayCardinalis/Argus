using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus.Migrations
{
    /// <inheritdoc />
    public partial class RestrictHistoryRelatedDeletions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_placement_histories_auditoriums_auditorium_id",
                table: "placement_histories");

            migrationBuilder.DropForeignKey(
                name: "fk_placement_histories_equipments_equipment_id",
                table: "placement_histories");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_auditoriums_auditorium_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_equipments_equipment_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_users_client_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_users_executor_id",
                table: "support_requests");

            migrationBuilder.AddForeignKey(
                name: "fk_placement_histories_auditoriums_auditorium_id",
                table: "placement_histories",
                column: "auditorium_id",
                principalTable: "auditoriums",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_placement_histories_equipments_equipment_id",
                table: "placement_histories",
                column: "equipment_id",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_auditoriums_auditorium_id",
                table: "support_requests",
                column: "auditorium_id",
                principalTable: "auditoriums",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_equipments_equipment_id",
                table: "support_requests",
                column: "equipment_id",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_users_client_id",
                table: "support_requests",
                column: "client_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_users_executor_id",
                table: "support_requests",
                column: "executor_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_placement_histories_auditoriums_auditorium_id",
                table: "placement_histories");

            migrationBuilder.DropForeignKey(
                name: "fk_placement_histories_equipments_equipment_id",
                table: "placement_histories");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_auditoriums_auditorium_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_equipments_equipment_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_users_client_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_users_executor_id",
                table: "support_requests");

            migrationBuilder.AddForeignKey(
                name: "fk_placement_histories_auditoriums_auditorium_id",
                table: "placement_histories",
                column: "auditorium_id",
                principalTable: "auditoriums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_placement_histories_equipments_equipment_id",
                table: "placement_histories",
                column: "equipment_id",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_auditoriums_auditorium_id",
                table: "support_requests",
                column: "auditorium_id",
                principalTable: "auditoriums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_equipments_equipment_id",
                table: "support_requests",
                column: "equipment_id",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_users_client_id",
                table: "support_requests",
                column: "client_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_users_executor_id",
                table: "support_requests",
                column: "executor_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}

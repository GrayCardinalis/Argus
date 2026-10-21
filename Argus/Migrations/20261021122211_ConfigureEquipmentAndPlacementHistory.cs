using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureEquipmentAndPlacementHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_placement_histories_equipment_id",
                table: "placement_histories");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "equipments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "equipments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "model_name",
                table: "equipments",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                table: "equipments",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "inventory_number",
                table: "equipments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_placement_histories_equipment_id",
                table: "placement_histories",
                column: "equipment_id",
                unique: true,
                filter: "removed_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_equipments_inventory_number",
                table: "equipments",
                column: "inventory_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_placement_histories_equipment_id",
                table: "placement_histories");

            migrationBuilder.DropIndex(
                name: "ix_equipments_inventory_number",
                table: "equipments");

            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "equipments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "equipments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "model_name",
                table: "equipments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                table: "equipments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "inventory_number",
                table: "equipments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "ix_placement_histories_equipment_id",
                table: "placement_histories",
                column: "equipment_id");
        }
    }
}

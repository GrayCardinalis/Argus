using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Argus.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auditorium",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    building_number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auditorium", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "component",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    quantity = table.Column<int>(type: "integer", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_component", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    inventory_number = table.Column<string>(type: "text", nullable: false),
                    model_name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    department = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "placement_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    auditorium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    installed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    removed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_placement_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_placement_history_auditorium_auditorium_id",
                        column: x => x.auditorium_id,
                        principalTable: "auditorium",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_placement_history_equipment_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    auditorium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    executor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_request_auditorium_auditorium_id",
                        column: x => x.auditorium_id,
                        principalTable: "auditorium",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_request_equipment_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_request_users_client_id",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_request_users_executor_id",
                        column: x => x.executor_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "support_request_comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    support_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_request_comment", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_request_comment_support_request_support_request_id",
                        column: x => x.support_request_id,
                        principalTable: "support_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_request_comment_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_request_component",
                columns: table => new
                {
                    support_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    component_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_request_component", x => new { x.support_request_id, x.component_id });
                    table.CheckConstraint("CK_Quantity_Positive", "quantity > 0");
                    table.ForeignKey(
                        name: "fk_support_request_component_component_component_id",
                        column: x => x.component_id,
                        principalTable: "component",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_support_request_component_support_request_support_request_id",
                        column: x => x.support_request_id,
                        principalTable: "support_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_component_name",
                table: "component",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_placement_history_auditorium_id",
                table: "placement_history",
                column: "auditorium_id");

            migrationBuilder.CreateIndex(
                name: "ix_placement_history_equipment_id",
                table: "placement_history",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_auditorium_id",
                table: "support_request",
                column: "auditorium_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_client_id",
                table: "support_request",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_equipment_id",
                table: "support_request",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_executor_id",
                table: "support_request",
                column: "executor_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_comment_author_id",
                table: "support_request_comment",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_comment_support_request_id",
                table: "support_request_comment",
                column: "support_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_component_component_id",
                table: "support_request_component",
                column: "component_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "placement_history");

            migrationBuilder.DropTable(
                name: "support_request_comment");

            migrationBuilder.DropTable(
                name: "support_request_component");

            migrationBuilder.DropTable(
                name: "component");

            migrationBuilder.DropTable(
                name: "support_request");

            migrationBuilder.DropTable(
                name: "auditorium");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

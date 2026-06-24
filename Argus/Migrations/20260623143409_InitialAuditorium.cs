using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Argus.Migrations
{
    /// <inheritdoc />
    public partial class InitialAuditorium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoriums",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    building_number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auditoriums", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "components",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_components", x => x.id);
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
                name: "placement_histories",
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
                    table.PrimaryKey("pk_placement_histories", x => x.id);
                    table.ForeignKey(
                        name: "fk_placement_histories_auditoriums_auditorium_id",
                        column: x => x.auditorium_id,
                        principalTable: "Auditoriums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_placement_histories_equipment_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_requests",
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
                    table.PrimaryKey("pk_support_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_requests_auditoriums_auditorium_id",
                        column: x => x.auditorium_id,
                        principalTable: "Auditoriums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_requests_equipment_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_requests_users_client_id",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_requests_users_executor_id",
                        column: x => x.executor_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "support_request_comments",
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
                    table.PrimaryKey("pk_support_request_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_request_comments_support_requests_support_request_id",
                        column: x => x.support_request_id,
                        principalTable: "support_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_request_comments_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportRequestComponents",
                columns: table => new
                {
                    support_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    component_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_request_components", x => new { x.support_request_id, x.component_id });
                    table.CheckConstraint("CK_Quantity_Positive", "quantity > 0");
                    table.ForeignKey(
                        name: "fk_support_request_components_components_component_id",
                        column: x => x.component_id,
                        principalTable: "components",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_support_request_components_support_requests_support_request_id",
                        column: x => x.support_request_id,
                        principalTable: "support_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_placement_histories_auditorium_id",
                table: "placement_histories",
                column: "auditorium_id");

            migrationBuilder.CreateIndex(
                name: "ix_placement_histories_equipment_id",
                table: "placement_histories",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_comments_author_id",
                table: "support_request_comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_comments_support_request_id",
                table: "support_request_comments",
                column: "support_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_requests_auditorium_id",
                table: "support_requests",
                column: "auditorium_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_requests_client_id",
                table: "support_requests",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_requests_equipment_id",
                table: "support_requests",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_requests_executor_id",
                table: "support_requests",
                column: "executor_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_components_component_id",
                table: "SupportRequestComponents",
                column: "component_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "placement_histories");

            migrationBuilder.DropTable(
                name: "support_request_comments");

            migrationBuilder.DropTable(
                name: "SupportRequestComponents");

            migrationBuilder.DropTable(
                name: "components");

            migrationBuilder.DropTable(
                name: "support_requests");

            migrationBuilder.DropTable(
                name: "Auditoriums");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

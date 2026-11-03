using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentQuantityNonNegative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Components_Quantity_NonNegative",
                table: "components",
                sql: "quantity >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Components_Quantity_NonNegative",
                table: "components");
        }
    }
}

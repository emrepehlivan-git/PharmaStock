using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaStock.Modules.Product.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComplianceAuditLogEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "compliance_audit_log_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aggregate_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    aggregate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operation_type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    previous_value = table.Column<string>(type: "text", nullable: true),
                    new_value = table.Column<string>(type: "text", nullable: true),
                    reason = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    performed_by_user_id = table.Column<string>(type: "text", nullable: true),
                    occurred_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_compliance_audit_log_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    unit_of_measurement = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    manufacturer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    batch_tracking_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    expiration_tracking_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    serial_tracking_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    cold_chain_required = table.Column<bool>(type: "boolean", nullable: false),
                    minimum_temperature_celsius = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    maximum_temperature_celsius = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    critical_stock_level = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_products_code",
                table: "products",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "compliance_audit_log_entries");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Persistence.Migrations
{
    public partial class RemovingOutboxMessageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntegrationEventType",
                table: "OutboxMessages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IntegrationEventType",
                table: "OutboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

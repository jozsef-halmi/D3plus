using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Persistence.Migrations
{
    public partial class Adding_Outbox2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutboxMessages_IntegrationEvent_IntegrationEventId",
                table: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "IntegrationEvent");

            migrationBuilder.DropIndex(
                name: "IX_OutboxMessages_IntegrationEventId",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "IntegrationEventId",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<string>(
                name: "IntegrationEventJson",
                table: "OutboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IntegrationEventType",
                table: "OutboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntegrationEventJson",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "IntegrationEventType",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "IntegrationEventId",
                table: "OutboxMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "IntegrationEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEvent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_IntegrationEventId",
                table: "OutboxMessages",
                column: "IntegrationEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutboxMessages_IntegrationEvent_IntegrationEventId",
                table: "OutboxMessages",
                column: "IntegrationEventId",
                principalTable: "IntegrationEvent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

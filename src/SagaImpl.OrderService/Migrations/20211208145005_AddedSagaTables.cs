using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaImpl.OrderService.Migrations
{
    public partial class AddedSagaTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Saga");

            migrationBuilder.CreateTable(
                name: "Definitions",
                schema: "Saga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    NumberOfPhases = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                schema: "Saga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SagaDefinitionId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TimeCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Definitions_SagaDefinitionId",
                        column: x => x.SagaDefinitionId,
                        principalSchema: "Saga",
                        principalTable: "Definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Steps",
                schema: "Saga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefinitionId = table.Column<int>(type: "int", nullable: false),
                    Phase = table.Column<int>(type: "int", nullable: false),
                    TransactionMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CompensationMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Steps_Definitions_DefinitionId",
                        column: x => x.DefinitionId,
                        principalSchema: "Saga",
                        principalTable: "Definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "Saga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    LogType = table.Column<byte>(type: "tinyint", nullable: false),
                    Log = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "Saga",
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_SessionId",
                schema: "Saga",
                table: "Logs",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SagaDefinitionId",
                schema: "Saga",
                table: "Sessions",
                column: "SagaDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_DefinitionId",
                schema: "Saga",
                table: "Steps",
                column: "DefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs",
                schema: "Saga");

            migrationBuilder.DropTable(
                name: "Steps",
                schema: "Saga");

            migrationBuilder.DropTable(
                name: "Sessions",
                schema: "Saga");

            migrationBuilder.DropTable(
                name: "Definitions",
                schema: "Saga");
        }
    }
}

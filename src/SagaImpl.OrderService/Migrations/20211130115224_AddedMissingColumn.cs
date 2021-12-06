using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaImpl.OrderService.Migrations
{
    public partial class AddedMissingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "OrderService",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "OrderService",
                table: "Orders");
        }
    }
}

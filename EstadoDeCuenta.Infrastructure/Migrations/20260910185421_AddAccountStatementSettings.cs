using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstadoDeCuenta.Infrastructure.Migrations
{
    public partial class AddAccountStatementSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountStatementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterestPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MinimumPaymentPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatementSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AccountStatementSettings",
                columns: new[] { "Id", "InterestPercentage", "MinimumPaymentPercentage" },
                values: new object[] { 1, 25m, 5m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountStatementSettings");
        }
    }
}

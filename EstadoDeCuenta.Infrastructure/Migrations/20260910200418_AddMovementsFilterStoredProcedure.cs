using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstadoDeCuenta.Infrastructure.Migrations
{
    public partial class AddMovementsFilterStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.usp_GetMovementsFiltered
    @FromDate DATETIME2 = NULL,
    @ToDate   DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.MovementId,
        m.CardId,
        m.MovementDate,
        m.MovementAmount,
        m.MovementDescription,
        m.MovementType
    FROM dbo.Movements AS m
    WHERE (@FromDate IS NULL OR m.MovementDate >= @FromDate)
      AND (@ToDate   IS NULL OR m.MovementDate <  @ToDate)
    ORDER BY m.MovementDate DESC;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_GetMovementsFiltered;");
        }
    }
}

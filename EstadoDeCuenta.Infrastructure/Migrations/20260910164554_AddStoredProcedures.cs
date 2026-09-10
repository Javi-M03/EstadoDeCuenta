using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstadoDeCuenta.Infrastructure.Migrations
{
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.usp_GetMovementsByCard
    @CardId INT
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
    WHERE m.CardId = @CardId
    ORDER BY m.MovementDate DESC;
END;");

            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.usp_GetCardsByClient
    @ClientId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.CardId,
        c.CardNumber,
        c.CardLimit,
        c.ClientId
    FROM dbo.Cards AS c
    WHERE c.ClientId = @ClientId
    ORDER BY c.CardId;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_GetMovementsByCard;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_GetCardsByClient;");
        }
    }
}

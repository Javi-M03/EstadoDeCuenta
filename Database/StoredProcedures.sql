/* =============================================================================
   EstadoDeCuenta - Stored Procedures
   Database: EstadoDeCuentaDB (SQL Server)

   These procedures back the read paths consumed by the REST API:
     - usp_GetMovementsByCard         : movements for a card, newest first
     - usp_GetCardsByClient           : cards that belong to a client
     - usp_GetMovementsByCardFiltered : movements for a card, filtered by an
                                        optional date range and/or type
                                        (transaction history screen)
     - usp_GetMovementsFiltered       : all movements (any card), filtered by an
                                        optional date range (movements screen)

   They are also created automatically by the EF Core migration
   "AddStoredProcedures". This script is provided as a standalone deliverable
   so the database can be provisioned without the application (run it in SSMS
   or sqlcmd against EstadoDeCuentaDB after the schema/migrations exist).
   ============================================================================= */

USE EstadoDeCuentaDB;
GO

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
END;
GO

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
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetMovementsByCardFiltered
    @CardId       INT,
    @FromDate     DATETIME2 = NULL,   -- inclusive lower bound; NULL = no lower bound
    @ToDate       DATETIME2 = NULL,   -- exclusive upper bound; NULL = no upper bound
    @MovementType INT       = NULL    -- 0 = Compra, 1 = Pago; NULL = both
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
      AND (@FromDate     IS NULL OR m.MovementDate >= @FromDate)
      AND (@ToDate       IS NULL OR m.MovementDate <  @ToDate)
      AND (@MovementType IS NULL OR m.MovementType =  @MovementType)
    ORDER BY m.MovementDate DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetMovementsFiltered
    @FromDate DATETIME2 = NULL,   -- inclusive lower bound; NULL = no lower bound
    @ToDate   DATETIME2 = NULL    -- exclusive upper bound; NULL = no upper bound
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
END;
GO

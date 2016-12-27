CREATE TYPE [HouseSales].[PricePaidDataType] AS TABLE (
    [TransactionId]    UNIQUEIDENTIFIER NOT NULL,
    [PropertyId]       INT              NOT NULL,
    [Price]            DECIMAL (18)     NOT NULL,
    [DateOfTransfer]   DATETIME         NOT NULL,
    [Postcode]         NVARCHAR (10)    NOT NULL,
    [PropertyType]     TINYINT          NOT NULL,
    [NewBuild]         TINYINT          NOT NULL,
    [Holding]          TINYINT          NOT NULL,
    [PrimaryAddress]   NVARCHAR (MAX)   NOT NULL,
    [SecondaryAddress] NVARCHAR (MAX)   NOT NULL,
    [Street]           NVARCHAR (100)   NOT NULL,
    [Locality]         NVARCHAR (100)   NOT NULL,
    [TownOrCity]       NVARCHAR (100)   NOT NULL,
    [District]         NVARCHAR (100)   NOT NULL,
    [County]           NVARCHAR (100)   NOT NULL,
    [PPDCategory]      TINYINT          NOT NULL,
    [RecordStatus]     TINYINT          NOT NULL);


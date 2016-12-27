CREATE TABLE [HouseSales].[PropertyTransaction] (
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
    [LastUpdated]      DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IDX_NC_PROPERTYID]
    ON [HouseSales].[PropertyTransaction]([PropertyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_NC_POSTCODE]
    ON [HouseSales].[PropertyTransaction]([Postcode] ASC);


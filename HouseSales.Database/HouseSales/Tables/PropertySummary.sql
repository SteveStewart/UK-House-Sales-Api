CREATE TABLE [HouseSales].[PropertySummary] (
    [PropertyId]      INT				NOT NULL,
    [TransactionId]   UNIQUEIDENTIFIER	NULL,
    [NumTransactions] INT				NULL,
    [LastUpdated]     DATETIME			NOT NULL,
	[PostcodeId]	  INT				NOT NULL
    PRIMARY KEY CLUSTERED ([PropertyId] ASC),
    FOREIGN KEY ([TransactionId]) REFERENCES [HouseSales].[PropertyTransaction] ([TransactionId]),
	FOREIGN KEY ([PostcodeId]) REFERENCES [HouseSales].[Postcode] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IDX_NC_POSTCODEID] ON [HouseSales].[PropertySummary]([PostcodeId] ASC)
GO
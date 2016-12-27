CREATE TABLE [HouseSales].[Postcode] (
    [Id]        INT			IDENTITY(1,1) NOT NULL,
    [Postcode]  VARCHAR (8) UNIQUE NOT NULL,
    [Latitude]  FLOAT (53)  NULL,
    [Longitude] FLOAT (53)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IDX_NC_POSTCODE] ON [HouseSales].[Postcode]([Postcode] ASC);

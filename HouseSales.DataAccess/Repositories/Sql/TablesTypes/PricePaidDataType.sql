
CREATE TYPE [HouseSales].[PricePaidDataType] AS TABLE(
	[TransactionId] [uniqueidentifier] NOT NULL,
	[PropertyId] [uniqueidentifier] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[DateOfTransfer] [datetime] NOT NULL,
	[Postcode] [nvarchar](10) NOT NULL,
	[PropertyType] [tinyint] NOT NULL,
	[NewBuild] [tinyint] NOT NULL,
	[Holding] [tinyint] NOT NULL,
	[PrimaryAddress] [nvarchar](max) NOT NULL,
	[SecondaryAddress] [nvarchar](max) NOT NULL,
	[Street] [nvarchar](100) NOT NULL,
	[Locality] [nvarchar](100) NOT NULL,
	[TownOrCity] [nvarchar](100) NOT NULL,
	[District] [nvarchar](100) NOT NULL,
	[County] [nvarchar](100) NOT NULL,
	[PPDCategory] [tinyint] NOT NULL,
	[RecordStatus] [tinyint] NOT NULL
)
GO



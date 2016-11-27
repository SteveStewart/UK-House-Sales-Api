CREATE TABLE [HouseSales].[PropertyTransaction]
(
	[TransactionId] [uniqueidentifier] PRIMARY KEY,
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
	[LastUpdated] [datetime] NOT NULL,

) ON [PRIMARY] 

GO



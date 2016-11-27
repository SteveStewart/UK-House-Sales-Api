
CREATE TABLE [HouseSales].[PropertySummary]
(
	[PropertyId] [uniqueidentifier] PRIMARY KEY,
	[TransactionId] [uniqueidentifier] NULL,
	[NumTransactions] [int] NULL,
	[LastUpdated] [datetime] NOT NULL,

) ON [PRIMARY]

GO

ALTER TABLE [HouseSales].[PropertySummary]  WITH CHECK ADD FOREIGN KEY([TransactionId])
	REFERENCES [HouseSales].[PropertyTransaction] ([TransactionId])
GO



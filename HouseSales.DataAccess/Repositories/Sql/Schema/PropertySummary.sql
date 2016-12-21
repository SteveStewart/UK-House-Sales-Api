
CREATE TABLE [HouseSales].[PropertySummary]
(
	[PropertyId] [INT] PRIMARY KEY,
	[TransactionId] [UNIQUEIDENTIFIER] NULL,
	[NumTransactions] [int] NULL,
	[LastUpdated] [datetime] NOT NULL,

) ON [PRIMARY]

GO

ALTER TABLE [HouseSales].[PropertySummary]  WITH CHECK ADD FOREIGN KEY([TransactionId])
	REFERENCES [HouseSales].[PropertyTransaction] ([TransactionId])
GO



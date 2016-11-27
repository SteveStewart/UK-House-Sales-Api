DROP PROCEDURE IF EXISTS [HouseSales].[GetTransactionsByPropertyId]
GO

CREATE PROCEDURE [HouseSales].[GetTransactionsByPropertyId]
	@PropertyId UNIQUEIDENTIFIER
AS 
BEGIN
	
	SELECT	[TransactionId]
			,[PropertyId]
			,[Price]
			,[DateOfTransfer]
			,[Postcode]
			,[PropertyType]
			,[NewBuild]
			,[Holding]
			,[PrimaryAddress]
			,[SecondaryAddress]
			,[Street]
			,[Locality]
			,[TownOrCity]
			,[District]
			,[County]
			,[PPDCategory]
			,[LastUpdated]
  FROM		[HouseSales].[PropertyTransaction] WITH(NOLOCK)
  WHERE		PropertyId = @PropertyId

END
GO



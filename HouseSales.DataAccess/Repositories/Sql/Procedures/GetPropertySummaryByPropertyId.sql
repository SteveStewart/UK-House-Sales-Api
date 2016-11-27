
DROP PROCEDURE IF EXISTS [HouseSales].[GetPropertySummaryByPropertyId]
GO

CREATE PROCEDURE [HouseSales].[GetPropertySummaryByPropertyId]
	@PropertyId UNIQUEIDENTIFIER
AS
BEGIN

	SELECT		summary.PropertyId, 
				trans.PrimaryAddress AS [PrimaryAddress],
				trans.SecondaryAddress AS [SecondaryAddress],
				trans.Street AS [Street],
				trans.Locality AS [Locality],
				trans.TownOrCity AS [TownOrCity],
				trans.District AS [District],
				trans.County AS [County],
				trans.Postcode AS [Postcode],				
				trans.Price AS [LastSalePrice],
				trans.DateOfTransfer AS [LastDateOfTransfer],
				trans.PropertyType AS [PropertyType],
				summary.NumTransactions AS [NumberOfTransactions]
	FROM		[HouseSales].PropertySummary AS summary WITH(NOLOCK)
	INNER JOIN	[HouseSales].PropertyTransaction AS trans WITH(NOLOCK) ON summary.PropertyId = trans.PropertyId AND summary.TransactionId = trans.TransactionId
	WHERE		@PropertyId = summary.PropertyId

END

GO



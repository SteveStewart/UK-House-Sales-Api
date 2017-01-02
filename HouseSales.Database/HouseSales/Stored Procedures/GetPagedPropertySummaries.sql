-- EXEC [HouseSales].[GetPagedPropertySummaries] 'EC3%',1, 25, NULL, NULL, 0,1

CREATE PROCEDURE [HouseSales].[GetPagedPropertySummaries]
	@Postcode		NVARCHAR(10) = '',
	@Page			INT	= 0,
	@RowsPerPage	INT = 25,
	@PropertyType	TINYINT = NULL,		-- All = NULL, Detached = 0, SemiDetached = 1, Terraced = 2, FlatOrMaisonette = 3, Other = 4
	@SoldAfter		DATETIME = NULL,
	@OrderField		TINYINT = NULL,		-- Address = 1, LastSaleDate = 2, LastSalePrice = 3
	@OrderDirection	TINYINT = NULL,		-- Ascending = 0, Descending = 1
	@ResultLimit	INT = 500
AS
BEGIN

	DECLARE @PostcodeId INT

	SELECT	@PostcodeId = Id 
	FROM	HouseSales.Postcode 
	WHERE	Postcode = @Postcode

	;WITH result AS 
	(
		SELECT		summary.PropertyId AS [PropertyId], 
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
		FROM		HouseSales.PropertySummary AS summary WITH(NOLOCK)
		INNER JOIN	HouseSales.PropertyTransaction AS trans WITH(NOLOCK) ON summary.PropertyId = trans.PropertyId AND summary.TransactionId = trans.TransactionId
		WHERE		PostcodeId = @PostcodeId
		AND			(@PropertyType IS NULL OR @PropertyType = trans.PropertyType)
		AND			(@SoldAfter IS NULL OR @SoldAfter < trans.DateOfTransfer)

	), resultCount AS 
	(	
		SELECT	COUNT(1) AS TotalRows 
		FROM	( SELECT TOP(@ResultLimit) 1 AS total FROM result ) AS countLimit
	)

	SELECT		result.PropertyId AS [PropertyId], 
				result.PrimaryAddress AS [PrimaryAddress],
				result.SecondaryAddress AS [SecondaryAddress],
				result.Street AS [Street],
				result.Locality AS [Locality],
				result.TownOrCity AS [TownOrCity],
				result.District AS [District],
				result.County AS [County],
				result.Postcode AS [Postcode],				
				result.LastSalePrice AS [LastSalePrice],
				result.LastDateOfTransfer AS [LastDateOfTransfer],
				result.PropertyType AS [PropertyType],
				result.NumberOfTransactions AS [NumberOfTransactions],
				resultCount.TotalRows AS [TotalRows]
	FROM		result, resultCount
	ORDER BY 
		CASE WHEN @OrderField = 1 AND @OrderDirection = 0 THEN result.Postcode END ASC,
		CASE WHEN @OrderField = 1 AND @OrderDirection = 1 THEN result.Postcode END DESC,
		CASE WHEN @OrderField = 2 AND @OrderDirection = 0 THEN result.LastDateOfTransfer END ASC,
		CASE WHEN @OrderField = 2 AND @OrderDirection = 1 THEN result.LastDateOfTransfer END DESC,
		CASE WHEN @OrderField = 3 AND @OrderDirection = 0 THEN result.LastSalePrice END ASC,
		CASE WHEN @OrderField = 3 AND @OrderDirection = 1 THEN result.LastSalePrice END DESC
	OFFSET		(@Page * @RowsPerPage) ROWS FETCH NEXT @RowsPerPage ROWS ONLY
		
END


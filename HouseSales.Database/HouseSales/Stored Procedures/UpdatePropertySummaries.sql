CREATE PROCEDURE [HouseSales].[UpdatePropertySummaries]
	@batch As [HouseSales].[PricePaidDataType] READONLY
AS
BEGIN
	
	-- Add any postcodes that are missing from the postcodes table but exist in a transaction record
	INSERT 
	INTO		HouseSales.Postcode	(Postcode, Latitude, Longitude)
	SELECT		DISTINCT trans.Postcode, 0,0
	FROM		HouseSales.PropertyTransaction AS trans WITH(NOLOCK) 
	INNER JOIN	@batch AS batch ON batch.TransactionId = trans.TransactionId
	LEFT JOIN	HouseSales.Postcode AS pcode WITH(NOLOCK) ON trans.Postcode = pcode.Postcode
	WHERE		pcode.Postcode IS NULL

	CREATE TABLE #tmpSummaryTable
	(
		PropertyId			INT NOT NULL,
		TransactionId		UNIQUEIDENTIFIER NOT NULL,
		PostcodeId			INT NOT NULL,
		NumTransactions		INT
	)
	
	-- Get the details of the property summary from the last transaction for each property Id
	INSERT INTO		#tmpSummaryTable
	SELECT			DISTINCT	trans.PropertyId, 
								trans.TransactionId,
								pcode.Id,
								(SELECT COUNT(1) FROM [HouseSales].PropertyTransaction AS c WITH(NOLOCK) WHERE c.PropertyId = trans.PropertyId)
	FROM			[HouseSales].PropertyTransaction AS trans WITH(NOLOCK)
	INNER JOIN		@batch AS batch ON batch.TransactionId = trans.TransactionId
	INNER JOIN		[HouseSales].[Postcode] AS pcode ON pcode.Postcode = trans.Postcode
	WHERE			trans.TransactionId = (	SELECT TOP 1 in_trans.TransactionId
											FROM [HouseSales].PropertyTransaction AS in_trans WITH(NOLOCK) 
											WHERE in_trans.PropertyId = trans.PropertyId
											ORDER BY DateOfTransfer DESC	)

	-- Merge the new results into the property summary.
	MERGE	[HouseSales].[PropertySummary] WITH(HOLDLOCK) AS target
	USING	#tmpSummaryTable AS source ON (target.PropertyId = source.PropertyId)
	WHEN 	NOT MATCHED BY target THEN 
				INSERT (PropertyId, TransactionId, PostcodeId, NumTransactions, LastUpdated)
				VALUES(source.PropertyId, source.TransactionId, source.PostcodeId, source.NumTransactions, GETDATE())
	WHEN	MATCHED AND (source.TransactionId != target.TransactionId) THEN
				UPDATE
				SET		target.TransactionId = source.TransactionId,
						target.PostcodeId = source.PostcodeId,
						target.NumTransactions = source.NumTransactions, 
						target.LastUpdated = GETDATE();

	DROP TABLE #tmpSummaryTable

END

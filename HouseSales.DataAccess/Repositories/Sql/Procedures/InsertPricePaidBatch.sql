DROP PROCEDURE IF EXISTS [HouseSales].[InsertPricePaidBatch]
GO

CREATE PROCEDURE [HouseSales].[InsertPricePaidBatch]
	@batch As [HouseSales].[PricePaidDataType] READONLY
AS
BEGIN
	
	DECLARE @LastUpdate DATETIME = GETDATE()
	
	CREATE TABLE #tmpAdditions	(TransactionID UNIQUEIDENTIFIER)
	
	DELETE 
	FROM		[HouseSales].[PropertyTransaction]
	WHERE		TransactionId IN ( SELECT TransactionId FROM @batch WHERE RecordStatus = 2	)

	UPDATE		original
	SET			[PropertyId] = batch.PropertyId, 
				[Price] = batch.Price, 
				[DateOfTransfer] = batch.DateOfTransfer, 
				[Postcode] = batch.Postcode, 
				[PropertyType] = batch.PropertyType, 
				[NewBuild] = batch.NewBuild, 
				[Holding] = batch.Holding, 
				[PrimaryAddress] = batch.PrimaryAddress, 
				[SecondaryAddress] = batch.SecondaryAddress, 
				[Street] = batch.Street, 
				[Locality] = batch.Locality, 
				[TownOrCity] = batch.TownOrCity, 
				[District] = batch.District, 
				[County] = batch.County, 
				[PPDCategory] = batch.PPDCategory, 
				[LastUpdated] = @LastUpdate
	FROM		@batch AS batch
	INNER JOIN	[HouseSales].[PropertyTransaction] AS original ON batch.TransactionId = original.TransactionId
	AND			batch.RecordStatus = 1

	INSERT		INTO #tmpAdditions
	SELECT		batch.TransactionId
	FROM		@batch AS batch
	LEFT JOIN	[HouseSales].[PropertyTransaction] AS t ON t.TransactionId = batch.TransactionId
	WHERE		batch.RecordStatus = 0
	AND			t.TransactionId IS NULL
		
	INSERT INTO [HouseSales].[PropertyTransaction] 
				([TransactionId], [PropertyId], [Price], [DateOfTransfer], [Postcode], [PropertyType], [NewBuild], [Holding], [PrimaryAddress], [SecondaryAddress], [Street], [Locality], [TownOrCity], [District], [County], [PPDCategory], LastUpdated)
    SELECT		batch.TransactionId, batch.PropertyId, batch.Price, batch.DateOfTransfer, batch.Postcode, batch.PropertyType, batch.NewBuild, batch.Holding, batch.PrimaryAddress, batch.SecondaryAddress, batch.Street, batch.Locality, batch.TownOrCity, batch.District, batch.County, batch.PPDCategory, @LastUpdate
	FROM		@batch AS batch
	INNER JOIN	#tmpAdditions AS addition ON addition.TransactionId = batch.TransactionId
	WHERE		batch.RecordStatus = 0 -- Addition

	-- Split into separate sproc

	CREATE TABLE #tmpSummaryTableUpdates
	(
		PropertyId			UNIQUEIDENTIFIER NOT NULL,
		TransactionId		UNIQUEIDENTIFIER NOT NULL,
		NumTransactions		INT
	)

	INSERT INTO		#tmpSummaryTableUpdates
	SELECT			DISTINCT	trans.PropertyId, 
								trans.TransactionId,
					(SELECT COUNT(1) FROM [HouseSales].PropertyTransaction AS c WITH(NOLOCK) WHERE c.PropertyId = trans.PropertyId)
	FROM			[HouseSales].PropertyTransaction AS trans WITH(NOLOCK)
	INNER JOIN		@batch AS batch ON batch.PropertyId = trans.PropertyId
	WHERE			trans.TransactionId = (	SELECT TOP 1 in_trans.TransactionId
											FROM [HouseSales].PropertyTransaction AS in_trans WITH(NOLOCK) 
											WHERE in_trans.PropertyId = trans.PropertyId
											ORDER BY DateOfTransfer DESC	)

	MERGE	[HouseSales].[PropertySummary] WITH(HOLDLOCK) AS target
	USING	#tmpSummaryTableUpdates AS source ON (target.PropertyId = source.PropertyId)
	WHEN 	NOT MATCHED BY target THEN 
				INSERT (PropertyId, TransactionId, NumTransactions,LastUpdated)
				VALUES(source.PropertyId, source.TransactionId, source.NumTransactions, GETDATE())
	WHEN	MATCHED AND (source.TransactionId != target.TransactionId) THEN
				UPDATE
				SET		target.TransactionId = source.TransactionId,
						target.NumTransactions = source.NumTransactions, 
						target.LastUpdated = GETDATE();

	DROP TABLE #tmpSummaryTableUpdates
	DROP TABLE	#tmpAdditions

END
GO



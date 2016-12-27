
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

	EXEC HouseSales.UpdatePropertySummaries @batch

	DROP TABLE #tmpAdditions

END

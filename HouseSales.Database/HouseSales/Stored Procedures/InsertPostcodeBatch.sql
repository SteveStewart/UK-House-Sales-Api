CREATE PROCEDURE [HouseSales].[InsertPostcodeBatch]
	@batch AS [HouseSales].[PostcodeDataType] READONLY
AS
BEGIN

	MERGE	HouseSales.[Postcode]  WITH(HOLDLOCK) AS target
	USING	@batch AS source ON (target.Postcode = source.Postcode)
	WHEN 	NOT MATCHED BY target THEN 
		INSERT (Postcode, Latitude, Longitude)
		VALUES (source.Postcode, source.Latitude, source.Longitude)
	WHEN	MATCHED THEN
		UPDATE
		SET		target.Postcode = source.Postcode,
				target.Latitude = source.Latitude, 
				target.Longitude = source.Longitude;
END
GO
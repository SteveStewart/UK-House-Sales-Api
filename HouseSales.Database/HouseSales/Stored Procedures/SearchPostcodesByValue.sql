CREATE PROCEDURE [HouseSales].[SearchPostcodesByValue]
	@PostcodeValue AS VARCHAR(8),
	@Limit INT = 10
AS
BEGIN
	
	SELECT	TOP (@Limit) 
			Id,
			Postcode,
			Latitude,
			Longitude
	FROM	HouseSales.Postcode
	WHERE	Postcode LIKE @PostcodeValue + '%'

END
GO



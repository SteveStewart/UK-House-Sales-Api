CREATE PROCEDURE [HouseSales].[SearchPostcodesByValue]
	@PostcodeValue AS VARCHAR(8),
	@Limit INT = 10
AS
BEGIN
	
	SELECT	TOP (@Limit) 
			Id,
			Postcode AS [Value],
			Latitude AS [Latitude],
			Longitude AS [Longitude]
	FROM	HouseSales.Postcode
	WHERE	Postcode LIKE @PostcodeValue + '%'

END
GO



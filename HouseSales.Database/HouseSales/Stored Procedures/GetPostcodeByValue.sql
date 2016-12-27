CREATE PROCEDURE [HouseSales].[GetPostcodeByValue]
	@PostcodeValue VARCHAR(8)
AS
BEGIN

	SELECT	Id,
			Postcode,
			Latitude,
			Longitude
	FROM	HouseSales.Postcode
	WHERE	Postcode = @PostcodeValue

END
GO



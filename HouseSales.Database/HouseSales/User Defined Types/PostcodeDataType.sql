CREATE TYPE [HouseSales].[PostcodeDataType] AS TABLE
(
    [Postcode]  VARCHAR (8) NOT NULL,
    [Latitude]  FLOAT  NULL,
    [Longitude] FLOAT  NULL
)
GO
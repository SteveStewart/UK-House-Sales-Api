# UK-House-Sale-Api
An unfinished ASP.NET WebAPI project to provide access to the house sale data from the Land Registry.

This was mainly just a mess around with WebApi / Dapper / SQL (and previously MongoDb) and the land registry data which is available as a set of .csv files from 1995. The repository includes code for parsing the csvs, bulk loading them into an sql database, a repository for retrieving data from the database and a simple api to search and paginate the 20 million or so records based on postcode, property type, date of sale, etc.

The land registry data is available from here: https://www.gov.uk/government/collections/price-paid-data


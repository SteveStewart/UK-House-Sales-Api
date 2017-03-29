# UK-House-Sale-Api
An ASP.NET WebAPI project to provide access to the house sale data from the Land Registry.

This was mainly just a mess around with the land registry data which is available as a set of .csv files from 1995 and is updated monthly. Technology is WebApi / Dapper / SQL. This repository includes code for parsing the csvs, bulk loading them into an sql database, a repository for retrieving data from the database and a simple api to search and paginate the 20 million or so records based on postcode, property type, date of sale, etc. Hopefully somebody might find it useful.

The land registry data is available from here: https://www.gov.uk/government/collections/price-paid-data


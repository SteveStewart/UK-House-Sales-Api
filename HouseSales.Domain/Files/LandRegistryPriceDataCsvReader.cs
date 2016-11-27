using HouseSales.Domain;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace HouseSales.Domain.Files
{
    /// <summary>
    /// A csv reader for land registry price data
    /// </summary>
    public class LandRegistryPriceDataCsvReader : IHouseSaleDataFileReader
    {
        private readonly TextFieldParser _parser;

        private const int TransactionIdentifierIndex = 0;
        private const int PriceIndex = 1;
        private const int DateOfTransferIndex = 2;
        private const int PostcodeIndex = 3;
        private const int PropertyTypeIndex = 4;
        private const int NewBuildIndex = 5;
        private const int HoldingIndex = 6;
        private const int PrimaryAddressIndex = 7;
        private const int SecondaryAddressIndex = 8;
        private const int StreetIndex = 9;
        private const int LocalityIndex = 10;
        private const int TownOrCityIndex = 11;
        private const int DistrictIndex = 12;
        private const int CountyIndex = 13;
        private const int PPDCategoryIndex = 14;
        private const int RecordStatusIndex = 15;

        public LandRegistryPriceDataCsvReader(TextFieldParser parser)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");

            _parser = parser;
        }

        /// <summary>
        /// Retrieve the price paid records from a land registry .csv file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IEnumerable<HouseSaleDataCsvRow> GetRows()
        {
            _parser.TextFieldType = FieldType.Delimited;
            _parser.SetDelimiters(",");

            String[] currentRow = null;

            List<HouseSaleDataCsvRow> pricePaidRecords = new List<HouseSaleDataCsvRow>();

            while (!_parser.EndOfData)
            {
                currentRow = _parser.ReadFields();

                try
                {
                    HouseSaleDataCsvRow record = BuildPricePaidRecordFromRowFields(currentRow);
                    pricePaidRecords.Add(record);
                }
                catch(MalformedLineException ex)
                {
                    // TODO: NLog
                    Console.WriteLine("Row: {0}, Error: {1}", currentRow[TransactionIdentifierIndex], ex.Message);
                }
            }

            return pricePaidRecords.AsReadOnly();
        }

        /// <summary>
        /// Build a new PricePaidRecord from the row fields
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private HouseSaleDataCsvRow BuildPricePaidRecordFromRowFields(String[] fields)
        {
            HouseSaleDataCsvRow newRecord = new HouseSaleDataCsvRow();

            newRecord.Id = ParseTransactionIdentifier(fields[TransactionIdentifierIndex]);
            newRecord.Price = decimal.Parse(fields[PriceIndex]);
            newRecord.DateOfTransfer = DateTime.Parse(fields[DateOfTransferIndex]);
            newRecord.Postcode = fields[PostcodeIndex];
            newRecord.PropertyType = ParsePropertyType(fields[PropertyTypeIndex]);
            newRecord.NewBuild = ParseNewBuild(fields[NewBuildIndex]);
            newRecord.Holding = ParseHolding(fields[HoldingIndex]);
            newRecord.PrimaryAddressName = fields[PrimaryAddressIndex];
            newRecord.SecondaryAddressName = fields[SecondaryAddressIndex];
            newRecord.Street = fields[StreetIndex];
            newRecord.Locality = fields[LocalityIndex];
            newRecord.TownOrCity = fields[TownOrCityIndex];
            newRecord.District = fields[DistrictIndex];
            newRecord.County = fields[CountyIndex];
            newRecord.PPDCategory = ParsePPDCategory(fields[PPDCategoryIndex]);
            newRecord.RecordStatus = ParseRecordStatus(fields[RecordStatusIndex]);

            return newRecord;
        }

        private Guid ParseTransactionIdentifier(String input)
        {
            input = input.Replace("{", "");
            input = input.Replace("}", "");

            return Guid.Parse(input);
        }

        /// <summary>
        /// Parse the record status from an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private RecordStatusType ParseRecordStatus(String input)
        {
            if (input == null)
                throw new ArgumentNullException("input is null");
            if (input.Length != 1)
                throw new ArgumentOutOfRangeException("input.Length != 1");

            switch(input)
            {
                case "A":
                    return RecordStatusType.Addition;
                case "C":
                    return RecordStatusType.Update;
                case "D":
                    return RecordStatusType.Deletion;
                default:
                    throw new MalformedLineException("Record status is invalid");
            }
        }

        /// <summary>
        /// Parse the PPD Category from an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private PPDCategoryType ParsePPDCategory(String input)
        {
            if (input == null)
                throw new ArgumentNullException("input is null");
            if (input.Length != 1)
                throw new ArgumentOutOfRangeException("input.Length != 1");

            switch(input)
            {
                case "A":
                    return PPDCategoryType.StandardPricePaid;
                case "B":
                    return PPDCategoryType.AdditionalPricePaid;
                default:
                    throw new MalformedLineException("Invalid PPD Category type");
            }
        }

        /// <summary>
        /// Parse the holding type from an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private HoldingType ParseHolding(String input)
        {
            if (input == null)
                throw new ArgumentNullException("input is null");
            if (input.Length != 1)
                throw new ArgumentOutOfRangeException("input.Length != 1");

            switch (input)
            {
                case "F":
                    return HoldingType.Freehold;
                case "L":
                    return HoldingType.Leasehold;
                case "U":
                    return HoldingType.Unknown;
                default:
                    throw new MalformedLineException("Invalid holding type");
            }
        }

        /// <summary>
        /// Parse the new build property from an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool ParseNewBuild(String input)
        {
            String cleanedInput = input.Replace("N", bool.FalseString).Replace("Y", bool.TrueString);

            bool parseValue = false;

            if (!bool.TryParse(cleanedInput, out parseValue))
                throw new MalformedLineException("Invalid new build input");

            return bool.Parse(cleanedInput);
        }

        /// <summary>
        /// Parse the property type from an input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private PropertyType ParsePropertyType(String input)
        {
            if (input == null)
                throw new ArgumentNullException("input is null");
            if (input.Length != 1)
                throw new ArgumentOutOfRangeException("input.Length != 1");

            switch(input)
            {
                case "D":
                    return PropertyType.Detached;
                case "S":
                    return PropertyType.SemiDetached;
                case "T":
                    return PropertyType.Terraced;
                case "F":
                    return PropertyType.FlatOrMaisonette;
                case "O":
                    return PropertyType.Other;
                default:
                    throw new MalformedLineException("Invalid property type");
            }
        }
    }
}

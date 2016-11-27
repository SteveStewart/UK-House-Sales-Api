using Geolocation;
using Microsoft.VisualBasic.FileIO;
using Postcodes.Domain;
using System;
using System.Collections.Generic;

namespace Postcodes.Files
{
    /// <summary>
    /// An implementation of IPostcodeFileReader to retrieve data from a .csv file
    /// </summary>
    public class PostcodeCsvReader : IPostcodeFileReader
    {
        private const int IdIndex = 0;
        private const int PostcodeIndex = 1;
        private const int LatitudeIndex = 2;
        private const int LongitudeIndex = 3;

        public IEnumerable<Postcode> GetPostcodesFromFile(string path)
        {
            using (TextFieldParser reader = new TextFieldParser(path))
            {
                reader.TextFieldType = FieldType.Delimited;
                reader.SetDelimiters(",");

                String[] currentRow = null;

                List<Postcode> postCodeRecords = new List<Postcode>();

                // Skip header
                if (!reader.EndOfData)
                    reader.ReadFields();

                while (!reader.EndOfData)
                {
                    currentRow = reader.ReadFields();

                    try
                    {
                        Postcode record = BuildPostcodeFromFields(currentRow);
                        postCodeRecords.Add(record);
                    }
                    catch (MalformedLineException ex)
                    {
                        Console.WriteLine("Row: {0}, Error: {1}", currentRow[IdIndex], ex.Message);
                    }
                }

                return postCodeRecords.AsReadOnly();
            }
        }    

        private Postcode BuildPostcodeFromFields(String[] fields)
        {
            if (fields == null)
                throw new ArgumentNullException("fields is null");

            Postcode newPostcode = new Postcode();
            newPostcode.Id = int.Parse(fields[IdIndex]);
            newPostcode.Value = fields[PostcodeIndex];

            double latitude = double.Parse(fields[LatitudeIndex]);
            double longitude = double.Parse(fields[LongitudeIndex]);

            newPostcode.Location = new DecimalGeoCoordinate(latitude, longitude);

            return newPostcode;
        }
    }
}

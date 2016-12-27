using Geolocation;
using Microsoft.VisualBasic.FileIO;
using Postcodes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Postcodes.Files
{
    public class OnsPostcodeCsvReader : IPostcodeFileReader
    {
        private const int PostcodeIndex = 1;
        private const int LatitudeIndex = 51;
        private const int LongitudeIndex = 52;

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
                        Console.WriteLine("Row: {0}, Error: {1}", currentRow[PostcodeIndex], ex.Message);
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
            newPostcode.Value = fields[PostcodeIndex];
            newPostcode.Value = Regex.Replace(newPostcode.Value, @"\s+", " ");

            double latitude = double.Parse(fields[LatitudeIndex]);

            if (!DecimalGeoCoordinate.IsValidLatitude(latitude))
                latitude = 0f;

            double longitude = double.Parse(fields[LongitudeIndex]);

            if (!DecimalGeoCoordinate.IsValidLongitude(longitude))
                longitude = 0f;

            newPostcode.Location = new DecimalGeoCoordinate(latitude, longitude);

            return newPostcode;
        }
    }
}

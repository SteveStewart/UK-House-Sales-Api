using HouseSales.Files;
using Microsoft.Azure.WebJobs;
using Microsoft.VisualBasic.FileIO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Postcodes.Domain;
using Postcodes.Files;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace PricePaidData.Importer
{
    public class Program
    {
        private static readonly int _batchSize = 10000;

        public static void Main(string[] args)
        {
            JobHost host = new JobHost();           
            host.RunAndBlock();
        }

    /*    public static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"D:\Code\Projects\House Price Data\dataset", "pp-????.csv");

            var postcodeReader = new OnsPostcodeCsvReader();
            var postcodes = postcodeReader.GetPostcodesFromFile(@"D:\Code\Projects\House Price Data\postcode-data\ONSPD_MAY_2016_UK.csv");

            ImportPostcodes(postcodes);

            foreach (var file in files)
            {
                var csvRows = GetPricePaidDataFromLocalFile(file);
                ImportPricePaidCsvData(Path.GetFileName(file), csvRows);
            }

            Console.ReadKey();
        }*/
               
        [Singleton]
        public static void ImportFile([QueueTrigger("house-sales-import-queue")] String filename)
        {
            // Connect to the file storage and attempt to read file
            var cloudShare = GetCloudFileShare();
            var file = GetFile(cloudShare, filename);
            var csvRows = GetPricePaidDataFromCloudFile(file);

            ImportPricePaidCsvData(filename, csvRows);
        }

        private static void ImportPricePaidCsvData(String filename, List<HouseSaleDataCsvRow> csvRows)
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["HouseSalesSqlDb"];
            var sw = new Stopwatch();

            do
            {
                sw.Restart();
                var batch = csvRows.Take(_batchSize);

                using (var connection = new SqlConnection(dbConnectionString.ToString()))
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 2, 0)))
                {
                    connection.Open();

                    InsertProperties(connection, batch.ToList());
                    transactionScope.Complete();
                }

                csvRows.RemoveRange(0, csvRows.Count > _batchSize ? _batchSize : csvRows.Count);
                Console.WriteLine("({2}) Batch finished in {0}. {1} remaining...", sw.Elapsed, csvRows.Count, filename);

            } while (csvRows.Count > 0);
        }

        private static void ImportPostcodes(IEnumerable<Postcode> postcodes)
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["HouseSalesSqlDb"];
            var dataTable = GetPostcodeDataTable(postcodes);

            using (var connection = new SqlConnection(dbConnectionString.ToString()))
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 2, 0)))
            {
                connection.Open();

                InsertPostcodes(connection, postcodes);
                transactionScope.Complete();
            }
        }

        private static void InsertPostcodes(SqlConnection connection, IEnumerable<Postcode> postcodes)
        {
            var dataTable = GetPostcodeDataTable(postcodes);

            SqlCommand command = new SqlCommand("HouseSales.InsertPostcodeBatch", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 240;

            SqlParameter batchParam = new SqlParameter("@batch", SqlDbType.Structured);
            batchParam.Value = dataTable;

            command.Parameters.Add(batchParam);
            command.ExecuteNonQuery();
        }

        private static void UpdatePropertySummaries()
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["HouseSalesSqlDb"];

            using (var connection = new SqlConnection(dbConnectionString.ToString()))
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 2, 0)))
            {
                SqlCommand command = new SqlCommand("HouseSales.UpdatePropertySummaries", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 600;

                command.ExecuteNonQuery();
            }
        }

        private static DataTable GetPostcodeDataTable(IEnumerable<Postcode> postcodes)
        {
            DataTable table = new DataTable("PostcodeDataType");

            table.Columns.Add("Postcode", typeof(String));
            table.Columns.Add("Latitude", typeof(float));
            table.Columns.Add("Longitude", typeof(float));

            foreach (var postcode in postcodes)
            {
                table.Rows.Add(postcode.Value, postcode.Location.Latitude, postcode.Location.Longitude);
            }

            return table;
        }


        private static void InsertProperties(SqlConnection connection, List<HouseSaleDataCsvRow> properties)
        {
            var datatable = GetPropertyDataTable(properties);

            SqlCommand command = new SqlCommand("HouseSales.InsertPricePaidBatch", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 240;

            SqlParameter batchParam = new SqlParameter("@batch", SqlDbType.Structured);
            batchParam.Value = datatable;

            command.Parameters.Add(batchParam);
            command.ExecuteNonQuery();
        }

        private static DataTable GetPropertyDataTable(List<HouseSaleDataCsvRow> properties)
        {
            DataTable table = new DataTable("PricePaidDataType");

            table.Columns.Add("TransactionId", typeof(Guid));
            table.Columns.Add("PropertyId", typeof(int));
            table.Columns.Add("Price", typeof(Decimal));
            table.Columns.Add("DateOfTransfer", typeof(DateTime));
            table.Columns.Add("Postcode", typeof(String));
            table.Columns.Add("PropertyType", typeof(short));
            table.Columns.Add("NewBuild", typeof(short));
            table.Columns.Add("Holding", typeof(short));
            table.Columns.Add("PrimaryAddress", typeof(String));
            table.Columns.Add("SecondaryAddress", typeof(String));
            table.Columns.Add("Street", typeof(String));
            table.Columns.Add("Locality", typeof(String));
            table.Columns.Add("TownOrCity", typeof(String));
            table.Columns.Add("District", typeof(String));
            table.Columns.Add("County", typeof(String));
            table.Columns.Add("PPDCategory", typeof(short));
            table.Columns.Add("RecordStatus", typeof(short));

            foreach (var property in properties.OrderByDescending(p => p.DateOfTransfer))
            {
                table.Rows.Add(
                    property.Id,
                    GetPropertyIdFromRow(property),
                    property.Price,
                    property.DateOfTransfer,
                    property.Postcode,
                    property.PropertyType,
                    property.NewBuild,
                    property.Holding,
                    property.PrimaryAddressName,
                    property.SecondaryAddressName,
                    property.Street,
                    property.Locality,
                    property.TownOrCity,
                    property.District,
                    property.County,
                    property.PPDCategory,
                    property.RecordStatus);
            }


            return table;
        }

        /// <summary>
        /// Generate a property Id from a HouseSaleDataCsvRow
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static int GetPropertyIdFromRow(HouseSaleDataCsvRow row)
        {
            var hashString = (row.PrimaryAddressName + row.SecondaryAddressName + row.Postcode).GetHashCode().ToString();
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(hashString));

                return Math.Abs(BitConverter.ToInt32(hash,0));
            }
        }

        private static List<HouseSaleDataCsvRow> GetPricePaidDataFromLocalFile(String filePath)
        {
            Console.WriteLine("Loading csv '{0}'...", filePath);

            using (var fileStream = File.OpenRead(filePath))
            using (var parser = new TextFieldParser(fileStream))
            {
                var csvReader = new LandRegistryPriceDataCsvReader(parser);

                return csvReader.GetRows().ToList();
            }
        }

        private static List<HouseSaleDataCsvRow> GetPricePaidDataFromCloudFile(CloudFile file)
        {
            Console.WriteLine("Loading csv '{0}'...", file.Name);

            using (var fileStream = file.OpenRead())
            using (var parser = new TextFieldParser(fileStream))
            {
                var csvReader = new LandRegistryPriceDataCsvReader(parser);

                return csvReader.GetRows().ToList();
            }
        }

        private static CloudFile GetFile(CloudFileShare share, String fileName)
        {               
            var rootDirectory = share.GetRootDirectoryReference();

            var dataDirectoryName = ConfigurationManager.AppSettings["DataSubFolder"];    
            var dataDirectory = rootDirectory.GetDirectoryReference(dataDirectoryName);
                    
            return dataDirectory.GetFileReference(fileName);
        }

        private static CloudFileShare GetCloudFileShare()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"];
            var shareName = ConfigurationManager.AppSettings["FileShareName"];

            var storageAccount = CloudStorageAccount.Parse(connectionString.ToString());
            var fileClient = storageAccount.CreateCloudFileClient();
                 
            return fileClient.GetShareReference(shareName);
        }
    }
}

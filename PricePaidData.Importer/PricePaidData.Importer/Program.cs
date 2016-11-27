using HouseSales.Domain.Files;
using Microsoft.Azure.WebJobs;
using Microsoft.VisualBasic.FileIO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace PricePaidData.Importer
{
    public class Program
    {
        private static readonly int _batchSize = 2000;

        public static void Main(string[] args)
        {
            JobHost host = new JobHost();           
            host.RunAndBlock();
        }

      /*  public static void Main(string[] args)
        {
            ImportFile("test");
        }*/
               
        [Singleton]
        public static void ImportFile([QueueTrigger("house-sales-import-queue")] String filename)
        {
            // Connect to the file storage and attempt to read file
            var cloudShare = GetCloudFileShare();
            var file = GetFile(cloudShare, filename);
            var csvRows = GetPricePaidDataFromCloudFile(file);
            
            var dbConnectionString = ConfigurationManager.ConnectionStrings["AzureHouseSalesSqlDb"];
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
                                   
            } while (csvRows.Count > 0) ;
        }

        private static void InsertProperties(SqlConnection connection, List<HouseSaleDataCsvRow> properties)
        {
            var datatable = GetDataTable(properties);

            SqlCommand command = new SqlCommand("InsertPricePaidBatch", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 240;

            SqlParameter batchParam = new SqlParameter("@batch", SqlDbType.Structured);
            batchParam.Value = datatable;

            command.Parameters.Add(batchParam);
            command.ExecuteNonQuery();
        }

        private static DataTable GetDataTable(List<HouseSaleDataCsvRow> properties)
        {
            DataTable table = new DataTable("PricePaidDataType");

            table.Columns.Add("TransactionId", typeof(Guid));
            table.Columns.Add("PropertyId", typeof(Guid));
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
        /// Generate a hash from a HouseSaleDataCsvRow
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Guid GetPropertyIdFromRow(HouseSaleDataCsvRow row)
        {
            var hashString = (row.PrimaryAddressName + row.SecondaryAddressName + row.Postcode).GetHashCode().ToString();
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(hashString));

                return new Guid(hash);
            }
        }

        private static List<HouseSaleDataCsvRow> GetPricePaidDataFromCloudFile(CloudFile file)
        {
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

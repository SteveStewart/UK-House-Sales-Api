using System.Collections.Generic;
using System.Threading.Tasks;
using Postcodes.Domain;
using System.Data.SqlClient;
using System;
using Dapper;
using System.Data;
using Geolocation;

namespace Postcodes.Repositories
{
    public class SqlServerPostcodeRepository : IPostcodeRepository
    {
        private readonly String _connectionString;

        public SqlServerPostcodeRepository(String connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            _connectionString = connectionString;
        }

        public async Task<Postcode> GetPostcode(string postcodeValue)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                String spName = "HouseSales.GetPostcodeByValue";
                return await connection.QuerySingleOrDefaultAsync<Postcode>(spName, new { PostcodeValue = postcodeValue }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Postcode>> Search(string postcodeValue, int limit = 10)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                String spName = "HouseSales.SearchPostcodesByValue";
                return await connection.QueryAsync<Postcode>(
                    sql: spName, 
                    param: new { PostcodeValue = postcodeValue, Limit = limit }, 
                    commandType: CommandType.StoredProcedure);
            }
        }

    }
}
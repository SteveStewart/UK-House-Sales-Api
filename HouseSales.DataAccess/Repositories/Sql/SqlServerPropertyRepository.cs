using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseSales.Domain;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace HouseSales.Repositories.SqlServer
{
    public class SqlServerPropertyRepository : IPropertyRepository
    {
        private readonly String _connectionString;

        public SqlServerPropertyRepository(String connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            _connectionString = connectionString;
        }

        public async Task<PaginatedResult<IEnumerable<PropertySummary>>> FindAsync(PropertyFindCriteria criteria, OrderBy orderBy, Paging paging)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                String spName = "HouseSales.GetPagedPropertySummaries";

                int totalRows = 0;

                var result = await connection.QueryAsync<PropertySummary, int, PropertySummary>(
                    sql: spName,
                    map: (summary, count) => { totalRows = count; return summary; },
                    param: new
                    {
                        Postcode = criteria.Postcode,
                        Page = paging.Page,
                        RowsPerPage = paging.PageLimit,
                        PropertyType = criteria.PropertyType == PropertyType.None ? null : criteria.PropertyType,
                        SoldAfter = criteria.LastSaleDate == DateTime.MinValue ? null : criteria.LastSaleDate,
                        OrderField = orderBy.Field,
                        OrderDirection = orderBy.Direction
                    },
                    splitOn: "TotalRows",
                    commandType: CommandType.StoredProcedure);                          

                return new PaginatedResult<IEnumerable<PropertySummary>>(result, paging.Page, paging.PageLimit, totalRows);
            }
        }

        public async Task<PropertySummary> GetSummaryById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                String spName = "HouseSales.GetPropertySummaryByPropertyId";                
                return await connection.QuerySingleAsync<PropertySummary>(spName, new { PropertyId = id }, commandType: CommandType.StoredProcedure);
            }       
        }

        public async Task<IEnumerable<PropertyTransaction>> GetTransactionsByPropertyId(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                String spName = "HouseSales.GetTransactionsByPropertyId";
                return await connection.QueryAsync<PropertyTransaction>(spName, new { PropertyId = Id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

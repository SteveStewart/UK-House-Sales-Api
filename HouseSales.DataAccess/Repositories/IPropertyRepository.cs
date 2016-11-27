using HouseSales.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseSales.Repositories
{
    public interface IPropertyRepository 
    {
        /// <summary>
        /// Get a property summary by the property Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PropertySummary> GetSummaryById(Guid id);

        /// <summary>
        /// Get the list of transactions for a property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<PropertyTransaction>> GetTransactionsByPropertyId(Guid id);              

        /// <summary>
        /// Find properties based on the search criteria
        /// </summary>
        /// <param name="criteria">The criteria to search on</param>
        /// <param name="order">The order to return the results</param>
        /// <param name="paging">The current page and number of items per page to return</param>
        /// <returns></returns>
        Task<PaginatedResult<IEnumerable<PropertySummary>>> FindAsync(PropertyFindCriteria criteria, OrderBy orderBy, Paging paging);
    }
}

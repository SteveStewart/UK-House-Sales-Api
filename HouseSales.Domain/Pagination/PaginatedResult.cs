using System;
using System.Collections;

namespace HouseSales.Domain
{
    public class PaginatedResult<T> where T : IEnumerable
    {
        public PaginatedResult(T results, int page, int pageLimit, int totalItems)
        {
            if (results == null)
                throw new ArgumentNullException("results is null");

            Page = page;
            TotalItems = totalItems;
            PageLimit = pageLimit;

            Results = results;
        }

        /// <summary>
        /// The result of the query
        /// </summary>
        public T Results { get; private set; }
        /// <summary>
        /// The current page
        /// </summary>
        public int Page { get; private set; }
        /// <summary>
        /// The total number of items in the paged list
        /// </summary>
        public int TotalItems { get; private set; }
        /// <summary>
        /// The number of items to display per page
        /// </summary>
        public int PageLimit { get; private set; }

        /// <summary>
        /// The total pages in the list
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (PageLimit == 0)
                    return 0;

                return (int)Math.Ceiling((decimal)TotalItems / PageLimit);
            }
        }

        /// <summary>
        /// The current start page index (i.e '50' of 100)
        /// </summary>
        public int PageStartIndex
        {
            get { return (Page * PageLimit) + 1; }
        }

        /// <summary>
        /// The current end page index (i.e. 50 of '100')
        /// </summary>
        public int PageEndIndex
        {
            get
            {
                int maxItems = PageStartIndex + PageLimit;

                return maxItems > TotalItems ? TotalItems : maxItems;
            }
        }
    }
}

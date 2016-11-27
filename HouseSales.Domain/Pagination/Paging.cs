namespace HouseSales.Domain
{
    /// <summary>
    /// Simple class to hold the details of paging
    /// </summary>
    public class Paging
    {
        public Paging(int page, int pageLimit)
        {
            Page = page;
            PageLimit = pageLimit;
        }

        /// <summary>
        /// The current page to retrieve
        /// </summary>
        public int Page { get; private set; }
        /// <summary>
        /// The maximum number of items per page
        /// </summary>
        public int PageLimit { get; private set; }
    }
}

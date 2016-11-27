using HouseSales.Domain;
using System;

namespace HouseSales.Repositories
{
    /// <summary>
    /// The criteria to use for find properties
    /// </summary>
    public class PropertyFindCriteria
    {
        /// <summary>
        /// The postcode to search on (full or partial)
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// The type of property to search for, i.e. Terraced
        /// </summary>
        public PropertyType? PropertyType { get; set; }
        /// <summary>
        /// The last sale date
        /// </summary>
        public DateTime? LastSaleDate { get; set; }   
        /// <summary>
        /// Limit the maximum possible number of results to return from the query
        /// </summary>
        public int? ResultLimit { get; set; }
    }
}
using HouseSales.Domain;
using HouseSales.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HouseSales.Web.Models
{
    public class PropertyListViewModel
    {
        /// <summary>
        /// The current search postcode
        /// </summary>
        public String Postcode { get; set; }

        /// <summary>
        /// The list of properties to display on the view
        /// </summary>
        public PaginatedResult<IEnumerable<PropertySummary>> PagedResults { get; set; }

        /// <summary>
        /// The list of options to sort the results by
        /// </summary>
        public IEnumerable<SelectListItem> SearchPeriodOptions { get; set; }
        
        /// <summary>
        /// The list of 'House Type' options to filter the results on
        /// </summary>
        public IEnumerable<SelectListItem> TypeFilterOptions { get; set; }

        /// <summary>
        /// Which field / direction to order the list by
        /// </summary>
        public OrderBy OrderBy { get; set; }

        /// <summary>
        /// The maximum number of results returned by the query
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Get the selected type filter
        /// </summary>
        public SelectListItem SelectedType
        {
            get
            {
                if (TypeFilterOptions == null)
                    TypeFilterOptions = new List<SelectListItem>();

                return TypeFilterOptions.FirstOrDefault(o => o.Selected);
            }
        }

        /// <summary>
        /// Get the selected period filter
        /// </summary>
        public SelectListItem SelectedPeriod
        {
            get
            {
                if (SearchPeriodOptions == null)
                    SearchPeriodOptions = new List<SelectListItem>();

                return SearchPeriodOptions.FirstOrDefault(o => o.Selected);
            }
        }
    }
}
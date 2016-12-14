using HouseSales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseSales.Web.Models
{
    public class PropertyDetailViewModel
    {
        /// <summary>
        /// The property summary
        /// </summary>
        public PropertySummary Summary { get; set; }
        /// <summary>
        /// The transactions for the property
        /// </summary>
        public IEnumerable<PropertyTransaction> Transactions { get; set; }
    }
}
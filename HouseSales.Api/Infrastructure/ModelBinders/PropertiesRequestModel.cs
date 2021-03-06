﻿using HouseSales.Domain;
using HouseSales.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace HouseSales.Api.Infrastructure
{
    public class PropertiesRequestModel
    {
        public PropertiesRequestModel()
        {
            SetDefaultValues();
        }

        /// <summary>
        /// The search postcode
        /// </summary>
        [Required]        
        public String Postcode { get; set; }
        /// <summary>
        /// The current page
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// The property search type
        /// </summary>
        public PropertyType PropertyType { get; set; }
        /// <summary>
        /// The search range in months (backwards)
        /// </summary>
        public int SearchRangeInMonths { get; set; }
        /// <summary>
        /// The order criteria
        /// </summary>
        public OrderBy OrderBy { get; set; }

        private void SetDefaultValues()
        {
            Postcode = String.Empty;
            PropertyType = PropertyType.None;
            OrderBy = new OrderBy(OrderField.None, OrderDirection.Ascending);
        }

    }
}
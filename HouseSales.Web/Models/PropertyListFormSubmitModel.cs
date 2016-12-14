using HouseSales.Domain;
using HouseSales.Repositories;
using System;

namespace HouseSales.Web.Models
{
    public class PropertyListFormSubmitModel
    {
        public PropertyListFormSubmitModel()
        {
            SetDefaultValues();
        }
        
        /// <summary>
        /// The search postcode
        /// </summary>
        public String Postcode { get; set; }
        /// <summary>
        /// The current page on the view
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

        /// <summary>
        /// Convert to the values to a PropertyFindCriteria object
        /// </summary>
        /// <param name="resultLimit">Limit the maximum number of results returned from the search.</param>
        /// <returns></returns>
        public PropertyFindCriteria ToFindCriteria(int resultLimit = 0)
        {
            return new PropertyFindCriteria()
            {
                Postcode = this.Postcode,
                PropertyType = this.PropertyType,
                LastSaleDate = this.SearchRangeInMonths <= 0 ? default(DateTime) : DateTime.Now.AddMonths(-this.SearchRangeInMonths),
                ResultLimit = resultLimit
            };
        }

        /// <summary>
        /// Quick and dirty validation method for the postcode.
        /// Needs converting into a validation attribute.
        /// </summary>
        public bool IsPostcodeFormatValid
        {
            get
            {

                if (String.IsNullOrEmpty(Postcode))
                    return false;
                if (Postcode.Length < 3 || Postcode.Length > 8)
                    return false;
                if (Postcode.Length > 4 && !Postcode.Contains(" "))
                    return false;

                return true;
            }
        }

        private void SetDefaultValues()
        {
            Postcode = String.Empty;
            PropertyType = PropertyType.None;
            OrderBy = new OrderBy(OrderField.None, OrderDirection.Ascending);
        }

    }
}
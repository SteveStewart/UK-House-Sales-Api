using System;
using System.Text;

namespace HouseSales.Domain
{
    public class PropertySummary
    {
        /// <summary>
        /// The internal unique reference for the property
        /// </summary>
        public int PropertyId { get; set; }
        /// <summary>
        /// The primary address of the property 
        /// </summary>
        public String PrimaryAddress { get; set; }
        /// <summary>
        /// The secondary address of the property, i.e. flat number
        /// </summary>
        public String SecondaryAddress { get; set; }
        /// <summary>
        /// The street string
        /// </summary>
        public String Street { get; set; }
        /// <summary>
        /// Locality of the property
        /// </summary>
        public String Locality { get; set; }
        /// <summary>
        /// The town / city of the property
        /// </summary>
        public String TownOrCity { get; set; }
        /// <summary>
        /// The district of the property
        /// </summary>
        public String District { get; set; }
        /// <summary>
        /// The county of the property
        /// </summary>
        public String County { get; set; }
        /// <summary>
        /// The postcode of the property
        /// </summary>
        public String Postcode { get; set; }
        /// <summary>
        /// The price the house last sold for
        /// </summary>
        public Decimal LastSalePrice { get; set; }
        /// <summary>
        /// The date of transfer for the last house sale
        /// </summary>
        public DateTime LastDateOfTransfer { get; set; }
        /// <summary>
        /// The type of property (at last sale)
        /// </summary>
        public PropertyType PropertyType { get; set; }
        /// <summary>
        /// The total number of sales
        /// </summary>
        public int NumberOfTransactions { get; set; }
        
        /// <summary>
        /// The geo-coordinate of the postcode
        /// </summary>
      //  public DecimalGeoCoordinate PostcodeLocation { get; set; }
           
        public String FullAddress
        {
            get
            {
                StringBuilder fullAddress = new StringBuilder();

                fullAddress.Append(PrimaryAddress);
                fullAddress.Append(!String.IsNullOrEmpty(SecondaryAddress) ? ", " + SecondaryAddress : String.Empty);
                fullAddress.Append(!String.IsNullOrEmpty(Street) ? ", " + Street : String.Empty);
                fullAddress.Append(!String.IsNullOrEmpty(Locality) ? ", " + Locality : String.Empty);
                fullAddress.Append(!String.IsNullOrEmpty(TownOrCity) ? ", " + TownOrCity : String.Empty);
                fullAddress.Append(!String.IsNullOrEmpty(County) ? ", " + County : String.Empty);
                fullAddress.Append(!String.IsNullOrEmpty(Postcode) ? ", " + Postcode : String.Empty);

                return fullAddress.ToString();
            }
        }

        public String ShortAddress
        {
            get
            {
                StringBuilder address = new StringBuilder();

                address.Append(PrimaryAddress);
                address.Append(!String.IsNullOrEmpty(Street) ? " " + Street : String.Empty);
                address.Append(!String.IsNullOrEmpty(Postcode) ? ", " + Postcode : String.Empty);

                return address.ToString();
            }
        }
    }
}

using HouseSales.Domain;
using System;

namespace HouseSales.Files
{
    /// <summary>
    /// Represents the price paid for a house
    /// </summary>
    public class HouseSaleDataCsvRow
    {
        /// <summary>
        /// The unique reference for an individual house sale record
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The price paid for the house
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The date of the transfer
        /// </summary>
        public DateTime DateOfTransfer { get; set; }
        /// <summary>
        /// The postcode of the property
        /// </summary>
        public String Postcode { get; set; }
        /// <summary>
        /// The type of property sold
        /// </summary>
        public PropertyType PropertyType { get; set; }
        /// <summary>
        /// The property was a new build for this transaction?
        /// </summary>
        public bool NewBuild { get; set; }
        /// <summary>
        /// Freehold or leasehold?
        /// </summary>
        public HoldingType Holding { get; set; }
        /// <summary>
        /// The primary address of the property 
        /// </summary>
        public String PrimaryAddressName { get; set; }
        /// <summary>
        /// The secondary address of the property, i.e. flat number
        /// </summary>
        public String SecondaryAddressName { get; set; }
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
        /// Sale category
        /// </summary>
        public PPDCategoryType PPDCategory { get; set;}
        /// <summary>
        /// The status of the record in the data, i.e. addition / deletion / update
        /// </summary>
        public RecordStatusType RecordStatus { get; set; }

    }
}

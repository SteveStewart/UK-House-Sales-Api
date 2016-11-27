using System;

namespace HouseSales.Domain
{
    public class PropertyTransaction
    {
        /// <summary>
        /// The unique reference for an individual house sale record
        /// </summary>
        public Guid TransactionId { get; set; }
        /// <summary>
        /// The date of the transfer
        /// </summary>
        public DateTime DateOfTransfer { get; set; }
        /// <summary>
        /// The price paid for the house
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The property was a new build for this transaction?
        /// </summary>
        public bool NewBuild { get; set; }
        /// <summary>
        /// Sale category
        /// </summary>
        public PPDCategoryType PPDCategory { get; set; }
        /// <summary>
        /// The type of property sold
        /// </summary>
        public PropertyType PropertyType { get; set; }
        /// <summary>
        /// Freehold or leasehold?
        /// </summary>
        public HoldingType Holding { get; set; }
    }
}

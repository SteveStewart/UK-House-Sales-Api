using Geolocation;
using System;

namespace Postcodes.Domain
{
    /// <summary>
    /// A class to hold a postcode value
    /// </summary>
    public class Postcode
    {
        /// <summary>
        /// The idea of the postcode
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The actual postcode value
        /// </summary>
        public String Value { get; set; }
        /// <summary>
        /// The approximate location of the postcode
        /// </summary>
        public DecimalGeoCoordinate Location { get; set; }
    }
}

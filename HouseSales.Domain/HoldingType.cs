using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseSales.Domain
{
    /// <summary>
    /// The holding type of the property
    /// </summary>
    public enum HoldingType
    {
        /// <summary>
        /// A freehold lease
        /// </summary>
        Freehold,
        /// <summary>
        /// A leasehold lease
        /// </summary>
        Leasehold,
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown
    }

}

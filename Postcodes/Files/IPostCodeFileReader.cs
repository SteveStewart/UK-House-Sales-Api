using Postcodes.Domain;
using System;
using System.Collections.Generic;

namespace Postcodes.Files
{
    /// <summary>
    /// An interface for a postcode file reader
    /// </summary>
    public interface IPostcodeFileReader
    {
        /// <summary>
        /// Get the postcode data from a specified file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IEnumerable<Postcode> GetPostcodesFromFile(String path);
    }
}

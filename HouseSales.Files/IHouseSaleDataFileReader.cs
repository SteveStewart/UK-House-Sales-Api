using HouseSales.Domain;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace HouseSales.Files
{
    /// <summary>
    /// Interface for a price data file reader
    /// </summary>
    public interface IHouseSaleDataFileReader
    {
        /// <summary>
        /// Retrieve the price paid records from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IEnumerable<HouseSaleDataCsvRow> GetRows();
    }
}
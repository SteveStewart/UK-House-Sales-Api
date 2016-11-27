using HouseSales.Infrastructure.Repository;
using Postcodes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postcodes.Repository
{
    public interface IPostcodeRepository : IRepository<Postcode, int>
    {
        /// <summary>
        /// Get all of the postcodes from the repository
        /// </summary>
        /// <returns></returns>
        IEnumerable<Postcode> GetAll();
    }
}

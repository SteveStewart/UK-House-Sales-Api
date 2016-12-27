using Postcodes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postcodes.Repositories
{
    public interface IPostcodeRepository
    {
        Task<Postcode> GetPostcode(String postcodeValue);
        Task<IEnumerable<Postcode>> Search(String postcodeValue, int limit = 10);
    }
}

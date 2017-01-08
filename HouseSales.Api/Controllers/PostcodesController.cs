using Postcodes.Domain;
using Postcodes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HouseSales.Api.Controllers
{
    public class PostcodesController : ApiController
    {
        private readonly IPostcodeRepository _postcodeRepository;

        public PostcodesController(IPostcodeRepository postcodeRepository)
        {
            _postcodeRepository = postcodeRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Postcode>> Search(String searchTerm, int resultLimit = 10)
        {
            if (String.IsNullOrWhiteSpace(searchTerm))
                return new List<Postcode>();

            return await _postcodeRepository.Search(searchTerm, resultLimit);  
        }
    }
}
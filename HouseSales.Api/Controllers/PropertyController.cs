using HouseSales.Domain;
using HouseSales.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HouseSales.Api.Controllers
{
    public class PropertyController : ApiController
    {
        private readonly IPropertyRepository _propertyRepository;
        public PropertyController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IHttpActionResult> Get(int propertyId)
        {
            var summary = await _propertyRepository.GetSummaryById(propertyId);

            if (summary == null)
                return NotFound();

            var transactions = await _propertyRepository.GetTransactionsByPropertyId(propertyId);

            var summaryAndTransactions = new PropertySummaryWithTransactions(summary, transactions);

            return Ok(summaryAndTransactions);
        }
    }
}

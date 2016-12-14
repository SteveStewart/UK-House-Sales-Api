using HouseSales.Domain;
using HouseSales.Repositories;
using HouseSales.Web.Models;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HouseSales.Web.Controllers
{
    public class PropertyDetailController : Controller
    {
        IPropertyRepository _propertyRepository;

        public PropertyDetailController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        // GET: PropertyDetail
        public async Task<ActionResult> Index(Guid propertyId)
        {
            var summary = await _propertyRepository.GetSummaryById(propertyId);
            var transactions = await _propertyRepository.GetTransactionsByPropertyId(propertyId);

            var viewModel = new PropertyDetailViewModel()
            {
                Summary = summary,
                Transactions = transactions
            };

            return View(viewModel);
        }
    }
}
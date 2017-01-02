using HouseSales.Api.Infrastructure;
using HouseSales.Api.Infrastructure.Swagger;
using HouseSales.Domain;
using HouseSales.Repositories;
using Postcodes.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace HouseSales.Api.Controllers
{
    public class PropertiesController : ApiController
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPostcodeRepository _postcodeRepository;

        public PropertiesController(IPropertyRepository propertyRepository, IPostcodeRepository postcodeRepository)
        {
            _propertyRepository = propertyRepository;
            _postcodeRepository = postcodeRepository;
        }

        /// <summary>
        /// Get a paged list of properties based on the specified criteria.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get([ModelBinder(typeof(PropertiesRequestModelBinder))] PropertiesRequestModel model)
        {
            ModelStateDictionary modelState = await ValidateModel(model);

            if (modelState.Count > 0)
                return BadRequest(modelState);

            var findCriteria = GetFindCriteria(model, 500);
            var orderBy = model.OrderBy;
            var paging = new Paging(model.Page, 25);

            var result = await _propertyRepository.FindAsync(findCriteria, orderBy, paging);                     

            return Ok(result);
        }

        private async Task<ModelStateDictionary> ValidateModel(PropertiesRequestModel model)
        {
            var modelStateDictionary = new ModelStateDictionary();

            var existingPostcode = await _postcodeRepository.GetPostcode(model.Postcode);

            if (existingPostcode == null)
                modelStateDictionary.AddModelError("Postcode", "Postcode does not exist.");

            return modelStateDictionary;
        }

        private PropertyFindCriteria GetFindCriteria(PropertiesRequestModel model, int resultLimit)
        {
            return new PropertyFindCriteria()
            {
                Postcode = model.Postcode,
                PropertyType = model.PropertyType,
                LastSaleDate = model.SearchRangeInMonths <= 0 ? default(DateTime) : DateTime.Now.AddMonths(-model.SearchRangeInMonths),
                ResultLimit = resultLimit
            };
        }
    }
}

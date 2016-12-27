using HouseSales.Api.Infrastructure;
using HouseSales.Domain;
using HouseSales.Repositories;
using Postcodes.Repositories;
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

        public async Task<IHttpActionResult> Get([ModelBinder(typeof(PropertiesRequestModelBinder))] PropertiesRequestModel model)
        {
            ModelStateDictionary modelState = await ValidateModel(model);

            if (modelState.Count > 0)
                return BadRequest(modelState);

            var findCriteria = model.ToFindCriteria(500);
            var orderBy = model.OrderBy;
            var paging = new Paging(model.Page, 25);

            var result = await _propertyRepository.FindAsync(findCriteria, orderBy, paging);                     

            return Ok(result);
        }

        public async Task<ModelStateDictionary> ValidateModel(PropertiesRequestModel model)
        {
            var modelStateDictionary = new ModelStateDictionary();

            var existingPostcode = await _postcodeRepository.GetPostcode(model.Postcode);

            if (existingPostcode == null)
                modelStateDictionary.AddModelError("Postcode", "Postcode does not exist.");

            return modelStateDictionary;
        }
    }
}

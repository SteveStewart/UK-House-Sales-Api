using HouseSales.Api.Infrastructure;
using HouseSales.Domain;
using HouseSales.Repositories;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace HouseSales.Api.Controllers
{
    public class PropertiesController : ApiController
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertiesController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IHttpActionResult> Get([ModelBinder(typeof(PropertiesRequestModelBinder))] PropertiesRequestModel model)
        {
            ModelStateDictionary modelState = null;

            if (!TryValidate(model, out modelState))
                return BadRequest(modelState);            

            var findCriteria = model.ToFindCriteria(500);
            var orderBy = model.OrderBy;
            var paging = new Paging(model.Page, 25);

            var result = await _propertyRepository.FindAsync(findCriteria, orderBy, paging);                     

            return Ok(result);
        }

        public bool TryValidate(PropertiesRequestModel model, out ModelStateDictionary modelState)
        {
            var modelStateDictionary = new ModelStateDictionary();
            bool isValid = true;
            
            if (model.Postcode.Length < 4)
            {
                isValid = false;
                modelStateDictionary.AddModelError("Postcode", "Postcode search string must be greater than 3 characters");
            }

            modelState = modelStateDictionary;
            return isValid;
        }
    }
}

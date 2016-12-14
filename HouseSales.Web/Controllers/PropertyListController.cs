using HouseSales.Domain;
using HouseSales.Repositories;
using HouseSales.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HouseSales.Web.Controllers
{
    public class PropertyListController : Controller
    {
        IPropertyRepository _propertyRepository;

        const int _pageLimit = 25;
        const int _resultLimit = 500;

        public PropertyListController(IPropertyRepository salesRepository)
        {
            if (salesRepository == null)
                throw new ArgumentNullException("salesRepository is null");

            _propertyRepository = salesRepository;
        }

        // GET: PropertyList
        public async Task<ActionResult> Index(PropertyListFormSubmitModel formModel)
        {        
            if (!formModel.IsPostcodeFormatValid)
            {
                PropertyListViewModel invalidViewModel = BuildInvalidViewModel(formModel);
                ViewData[Properties.Resources.TXT_POSTCODE_INVALID_VIEWDATA_KEY] = Properties.Resources.TXT_INVALID_POSTCODE_FORMAT;
             
                return View(invalidViewModel);
            }

            PropertyFindCriteria criteria = formModel.ToFindCriteria(_resultLimit);
            Paging paging = new Paging(formModel.Page, _pageLimit);
           
            var pagedResults = await _propertyRepository.FindAsync(criteria, formModel.OrderBy, paging);
            PropertyListViewModel viewModel = BuildViewModel(pagedResults, formModel);

            return View(viewModel);
        }       
                                             
        /// <summary>
        /// Build a view model for the property list view
        /// </summary>
        /// <param name="pagedResult"></param>
        /// <param name="formModel"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        private PropertyListViewModel BuildViewModel(PaginatedResult<IEnumerable<PropertySummary>> pagedResult, PropertyListFormSubmitModel formModel)
        {
            String searchRange = formModel.SearchRangeInMonths.ToString();
            String propertyType = ((int)formModel.PropertyType).ToString();

            PropertyListViewModel viewModel = new PropertyListViewModel()
            {
                PagedResults = pagedResult,
                Postcode = formModel.Postcode,
                SearchPeriodOptions = GetSearchPeriodFilterOptions(searchRange),
                TypeFilterOptions = GetTypeFilterOptions(propertyType),
                OrderBy = formModel.OrderBy,
                MaxResults = _resultLimit
            };

            return viewModel;
        }

        /// <summary>
        /// Build the invalid view model for validation failures
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private PropertyListViewModel BuildInvalidViewModel(PropertyListFormSubmitModel model)
        {
            var properties = new PaginatedResult<IEnumerable<PropertySummary>>(new List<PropertySummary>(), 0,0,0);

            return BuildViewModel(properties, model);
        }

        /// <summary>
        /// Get the search period filter options
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        private List<SelectListItem> GetSearchPeriodFilterOptions(String selectedValue = "")
        {
            if (selectedValue == String.Empty)
                selectedValue = "0";

            List<SelectListItem> periods = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "All sales history", Value="0" },
                new SelectListItem() { Text = "Last 12 months", Value="12" },
                new SelectListItem() { Text = "Last 2 years", Value="24" },
                new SelectListItem() { Text = "Last 5 years", Value="60" },
                new SelectListItem() { Text = "Last 10 years", Value="120" }
            };

            SelectListItem selectedPeriod = periods.FirstOrDefault(p => p.Value == selectedValue);

            if (selectedPeriod != null)
                selectedPeriod.Selected = true;

            return periods;
        }

        /// <summary>
        /// Get the property type filter options
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        private List<SelectListItem> GetTypeFilterOptions(String selectedValue = "")
        {
            List<SelectListItem> types = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Any property type", Value="-1" },
                new SelectListItem() { Text = "Detached", Value = "0" },
                new SelectListItem() { Text = "Semi-detached", Value = "1" },
                new SelectListItem() { Text = "Terraced", Value = "2" },
                new SelectListItem() { Text = "Flat or Maisonette", Value = "3" },
                new SelectListItem() { Text = "Other", Value = "4" }
            };

            SelectListItem selectedType = types.FirstOrDefault(t => t.Value == selectedValue);

            if (selectedType != null)
                selectedType.Selected = true;

            return types;
        }
    }
}
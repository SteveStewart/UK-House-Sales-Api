using HouseSales.Web.Models;
using System.Web.Mvc;

namespace HouseSales.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PropertyListFormSubmitModel model)
        {
            if (!model.IsPostcodeFormatValid)
            {
                ViewData[Properties.Resources.TXT_POSTCODE_INVALID_VIEWDATA_KEY] = Properties.Resources.TXT_INVALID_POSTCODE_FORMAT;
                return View();
            }

            return RedirectToAction("Index", "PropertyList", new { Postcode = model.Postcode });
        }
    }
}
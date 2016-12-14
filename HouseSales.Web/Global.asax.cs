using HouseSales.Web.Infrastructure.ModelBinders;
using HouseSales.Web.Models;
using System.Web.Mvc;
using System.Web.Routing;

namespace HouseSales.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-GB");

            RegisterModelBinders();
        }

        private void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(PropertyListFormSubmitModel), new PropertyListFormSubmitModelBinder());
        }
    }
}

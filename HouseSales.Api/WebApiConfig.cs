using HouseSales.Api.Infrastructure;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using WebApiContrib.IoC.Ninject;

namespace HouseSales.Api
{
    public class WebApiConfig
    {
        public static HttpConfiguration Register()
        {
            var config = new HttpConfiguration();

            config.DependencyResolver = new NinjectResolver(NinjectConfig.CreateKernel());
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            InitialiseJsonFormatter(config);

            return config;
        }

        private static void InitialiseModelBinders(HttpConfiguration config)
        {
            var provider = new SimpleModelBinderProvider(typeof(PropertiesRequestModel), new PropertiesRequestModelBinder());

            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
        }

        private static void InitialiseJsonFormatter(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
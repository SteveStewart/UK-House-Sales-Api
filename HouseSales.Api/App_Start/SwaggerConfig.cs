using System.Web.Http;
using WebActivatorEx;
using HouseSales.Api;
using Swashbuckle.Application;
using HouseSales.Api.Infrastructure.Swagger;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace HouseSales.Api
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            config 
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "HouseSales.Api");
                        c.OperationFilter<RemoveModelNameFilter>();         
                        c.IncludeXmlComments(string.Format(@"{0}\bin\HouseSales.Api.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                    })
                .EnableSwaggerUi(c =>  { });
        }
    }
}
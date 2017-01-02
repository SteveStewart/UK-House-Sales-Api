using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace HouseSales.Api.Infrastructure.Swagger
{
    /// <summary>
    /// Basic operation filter to remove the prefix from any parameters that use model binding
    /// TODO: This is not the best. 
    /// TODO: Add attributes to decorate properties on the model class and only replace params that are tagged with the attributes.
    /// </summary>
    public class RemoveModelNameFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            foreach (var param in operation.parameters)
            {
                var splitParam = param.name.Split('.');
                param.name = splitParam.LastOrDefault();
            }
        }
    }
}
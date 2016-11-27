using HouseSales.Repositories;
using HouseSales.Repositories.SqlServer;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HouseSales.Api.Infrastructure
{
    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            var connectionString = ConfigurationManager.ConnectionStrings["HouseSalesSqlDb"].ToString();

            kernel.Bind<IPropertyRepository>()
                .To<SqlServerPropertyRepository>().WithConstructorArgument("connectionString", connectionString.ToString());

            return kernel;
        }
    }
}
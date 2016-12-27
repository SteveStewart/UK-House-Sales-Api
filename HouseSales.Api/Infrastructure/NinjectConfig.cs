using HouseSales.Repositories;
using HouseSales.Repositories.SqlServer;
using Ninject;
using Postcodes.Repositories;
using System.Configuration;

namespace HouseSales.Api.Infrastructure
{
    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            var connectionString = ConfigurationManager.ConnectionStrings["HouseSalesSqlDb"].ToString();

            kernel.Bind<IPropertyRepository>()
                .To<SqlServerPropertyRepository>().WithConstructorArgument("connectionString", connectionString);

            kernel.Bind<IPostcodeRepository>()
                .To<SqlServerPostcodeRepository>().WithConstructorArgument("connectionString", connectionString);

            return kernel;
        }
    }
}
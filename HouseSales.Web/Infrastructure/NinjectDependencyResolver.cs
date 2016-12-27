using HouseSales.Domain;
using HouseSales.Repositories;
using HouseSales.Repositories.SqlServer;
using Ninject;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace HouseSales.Web.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;

            CreateBindings();
        }

        private void CreateBindings()
        {            
            _kernel.Bind<IPropertyRepository>().ToMethod<IPropertyRepository>(GetPropertyRepository);
        }
        
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private IPropertyRepository GetPropertyRepository(IContext context)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["HouseSalesSqlDb"].ToString();

            return new SqlServerPropertyRepository(connectionString);
        }

    }
}
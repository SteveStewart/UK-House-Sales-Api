using HouseSales.Repositories;
using HouseSales.Repositories.SqlServer;
using Microsoft.Owin;
using Ninject;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(HouseSales.Api.Startup))]

namespace HouseSales.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {                 
            app.UseWebApi(WebApiConfig.Register());
        }
    }
}
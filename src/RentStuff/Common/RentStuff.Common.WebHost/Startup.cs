﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.OwinHost;
using NLog;
using Owin;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Common.WebHost.Providers;
using RentStuff.Identity.Application.Ninject.Modules;
using RentStuff.Identity.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Identity.Infrastructure.Services.Ninject.Modules;
using RentStuff.Identity.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Services.Application.Ninject.Modules;
using RentStuff.Services.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Services.Ports.Adapter.Rest.Ninject.Modules;

[assembly: OwinStartup(typeof(RentStuff.Common.WebHost.Startup))]
namespace RentStuff.Common.WebHost
{
    public class AppBuilderProvider : IDisposable
    {
        private IAppBuilder _app;
        public AppBuilderProvider(IAppBuilder app)
        {
            _app = app;
        }
        public IAppBuilder Get() { return _app; }
        public void Dispose() { }
    }

    public class Startup
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new AppBuilderProvider(app));
            
            HttpConfiguration config = new HttpConfiguration();
            
            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                SupportsCredentials = true
            };

            policy.Origins.Add(Constants.FrontEndUrl);
            //policy.Origins.Add("http://localhost:11803/");
            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            });

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureOAuth(app);
            WebApiConfig.Register(config);
            //app.UseWebApi(config);
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);

            _logger.Info("ASP.NET and OWIN pipeline established");
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            // Common
            kernel.Load<CommonNinjectModule>();
            //Identity & Access
            kernel.Load<IdentityAccessServicesNinjectModule>();
            kernel.Load<IdentityAccessPersistenceNinjectModule>();
            kernel.Load<IdentityAccessApplicationNinjectModule>();
            kernel.Load<IdentityAccessPortsNinjectModule>();
            // Property
            kernel.Load<PropertyPersistenceNinjectModule>();
            kernel.Load<PropertyApplicationNinjectModule>();
            kernel.Load<PropertyPortsNinjectModule>();
            // Services
            kernel.Load<ServicePersistenceNinjectModule>();
            kernel.Load<ServiceApplicationNinjectModule>();
            kernel.Load<ServicePortsNinjectModule>();

            // Apply Ninject as the dependency resolver
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            return kernel;
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }
    }
}
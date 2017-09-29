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
using RentStuff.Identity.Ports.Adapter.Rest.Resources;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Ninject.Modules;
using System;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;

// Specify the startup point for OWIN, which is this Startup class
[assembly: OwinStartup(typeof(RentStuff.Common.WebHost.Startup))]
namespace RentStuff.Common.WebHost
{
    /// <summary>
    /// App Builder 
    /// </summary>
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

    /// <summary>
    /// Fires up the app
    /// </summary>
    public class Startup
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Specify the OWIN pipeline and other configurations
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            // Create context for OWIN
            app.CreatePerOwinContext(() => new AppBuilderProvider(app));
            
            // Initialize HTTP configuration
            HttpConfiguration config = new HttpConfiguration();
            
            // Cross-origin policy
            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                SupportsCredentials = true
            };

            policy.Origins.Add(Constants.FrontEndUrl);

            // Specify that we want to use Cors
            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            });
            
            // Configure OAuth for this application
            ConfigureOAuth(app);

            // Register Web Api
            WebApiConfig.Register(config);
            
            // Configure and load Ninject dependencies and specify to use it as with Web Api
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);
            
            _logger.Info("ASP.NET and OWIN pipeline established");
        }

        private static StandardKernel _kernel;

        /// <summary>
        /// Create the Kernel for Ninject
        /// </summary>
        /// <returns></returns>
        private static StandardKernel CreateKernel()
        {
            _kernel = new StandardKernel();
            // Common
            _kernel.Load<CommonNinjectModule>();
            //Identity & Access
            _kernel.Load<IdentityAccessServicesNinjectModule>();
            _kernel.Load<IdentityAccessPersistenceNinjectModule>();
            _kernel.Load<IdentityAccessApplicationNinjectModule>();
            _kernel.Load<IdentityAccessPortsNinjectModule>();
            // Property
            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<PropertyApplicationNinjectModule>();
            _kernel.Load<PropertyPortsNinjectModule>();

            // Apply Ninject as the dependency resolver
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(_kernel);
            
            return _kernel;
        }

        /// <summary>
        /// Configure OAuth for the app
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerAuthenticationOptions);

            // Facebook Authentication
            app.UseFacebookAuthentication(AccountController.FacebookAuthenticationOptions);
        }

        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        /* This code can be used at startup to invoke a recurring task that fires up at the specified intervals.
         Can be used later.
        private static CacheItemRemovedCallback OnCacheRemove = null;

        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }
        
        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            var houseApplicationService = _kernel.Get<IHouseApplicationService>();
            houseApplicationService.DeleteOutdatedHouses();
        }*/
    }
}
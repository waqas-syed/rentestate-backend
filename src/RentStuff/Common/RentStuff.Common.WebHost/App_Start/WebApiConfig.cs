using System.Web.Http;
//using Spring.Context.Support;

namespace RentStuff.Common.WebHost
{
    /// <summary>
    /// Configuration for Web Api
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register the configuration for Web Api
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors(new EnableCorsAttribute(Constants.DOMAINURL, "*", "*") { SupportsCredentials = true });
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Setting up the Json Formatter
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //config.DependencyResolver = new SpringDependencyResolver(ContextRegistry.GetContext());
            // Decipher password for the connection string and update it for using NHibernate.
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
        }
    }
}

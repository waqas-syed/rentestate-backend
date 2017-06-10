using System.Web.Http;
using Spring.Context.Support;

namespace RentStuff.Common.WebHost
{
    public static class WebApiConfig
    {
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

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.DependencyResolver = new SpringDependencyResolver(ContextRegistry.GetContext());
            // Decipher password for the connection string and update it for using NHibernate.
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
        }
    }
}

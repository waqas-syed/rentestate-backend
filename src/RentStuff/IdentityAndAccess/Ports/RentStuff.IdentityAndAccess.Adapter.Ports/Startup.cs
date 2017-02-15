using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RentStuff.IdentityAndAccess.Adapter.Ports.Startup))]
namespace RentStuff.IdentityAndAccess.Adapter.Ports
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

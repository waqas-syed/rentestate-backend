using Ninject.Modules;
using RentStuff.Identity.Ports.Adapter.Rest.Resources;

namespace RentStuff.Identity.Ports.Adapter.Rest.Ninject.Modules
{
    /// <summary>
    /// Dependencies declaration for IdentityAndAccess.Ports project
    /// </summary>
    public class IdentityAccessPortsNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<AccountController>().To<AccountController>().InTransientScope();
        }
    }
}

using Ninject.Modules;
using RentStuff.Services.Application.ApplicationServices;

namespace RentStuff.Services.Application.Ninject.Modules
{
    /// <summary>
    /// Ninject module for Application layer of Services Bounded Context
    /// </summary>
    public class ServiceApplicationNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<IServiceApplicationService>().To<ServiceApplicationService>();
        }
    }
}

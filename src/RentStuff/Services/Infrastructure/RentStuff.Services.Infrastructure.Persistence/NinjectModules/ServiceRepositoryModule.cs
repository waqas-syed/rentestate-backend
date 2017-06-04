using Ninject.Modules;

namespace RentStuff.Services.Infrastructure.Persistence.NinjectModules
{
    public class ServiceRepositoryModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            //Bind<IServicesRepository>().To<ServicesRepository>();
        }
    }
}

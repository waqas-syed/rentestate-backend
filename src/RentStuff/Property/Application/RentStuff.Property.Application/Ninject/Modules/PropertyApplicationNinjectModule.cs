using Ninject.Modules;
using RentStuff.Property.Application.HouseServices;

namespace RentStuff.Property.Application.Ninject.Modules
{
    /// <summary>
    /// Ninject dependency definer for the Property.Application project
    /// </summary>
    public class PropertyApplicationNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<IHouseApplicationService>().To<HouseApplicationService>().InTransientScope();
        }
    }
}

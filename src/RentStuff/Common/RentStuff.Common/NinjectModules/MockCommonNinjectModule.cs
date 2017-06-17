using Ninject.Modules;
using RentStuff.Common.Services.GoogleStorageServices;
using RentStuff.Common.Services.GoogleStorageServices.Mocks;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Common.Services.LocationServices.Mocks;

namespace RentStuff.Common.NinjectModules
{
    /// <summary>
    /// Ninject module for loading the Mock dependenciess
    /// </summary>
    public class MockCommonNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<IGeocodingService>().To<MockGeocodingService>().InTransientScope();
            Bind<IPhotoStorageService>().To<MockPhotoStorageService>().InTransientScope();
        }
    }
}

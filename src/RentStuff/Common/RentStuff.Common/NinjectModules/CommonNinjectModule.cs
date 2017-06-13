using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using RentStuff.Common.Services.GoogleStorageServices;
using RentStuff.Common.Services.LocationServices;

namespace RentStuff.Common.NinjectModules
{
    /// <summary>
    /// Module for declaring types for Ninject to prepare for Dependency Injection
    /// </summary>
    public class CommonNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<IGeocodingService>().To<GeocodingService>().InTransientScope();
            Bind<IPhotoStorageService>().To<PhotoStorageService>().InTransientScope();
        }
    }
}

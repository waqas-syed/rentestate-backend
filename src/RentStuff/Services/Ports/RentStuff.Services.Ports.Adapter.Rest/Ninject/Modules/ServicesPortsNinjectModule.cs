using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using RentStuff.Services.Ports.Adapter.Rest.Resources;

namespace RentStuff.Services.Ports.Adapter.Rest.Ninject.Modules
{
    /// <summary>
    /// Ninject DI provider for the Services.Ports layer
    /// </summary>
    public class ServicesPortsNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<ServiceController>().To<ServiceController>().InTransientScope();
        }
    }
}

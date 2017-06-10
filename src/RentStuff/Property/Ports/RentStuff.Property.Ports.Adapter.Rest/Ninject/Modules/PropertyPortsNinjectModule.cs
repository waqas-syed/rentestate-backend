using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Ninject.Web.Common;
using RentStuff.Property.Ports.Adapter.Rest.Resources;

namespace RentStuff.Property.Ports.Adapter.Rest.Ninject.Modules
{
    /// <summary>
    /// Defines the dependencies for the Property.Ports project
    /// </summary>
    public class PropertyPortsNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<HouseController>().To<HouseController>().InRequestScope();
        }
    }
}

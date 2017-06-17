using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using RentStuff.Identity.Infrastructure.Services.Email;

namespace RentStuff.Identity.Infrastructure.Services.Ninject.Modules
{
    /// <summary>
    /// Ninject Dependency definitions foe the IdentityAndAccess.InsfrastructureServices project
    /// </summary>
    public class IdentityAccessServicesNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<ICustomEmailService>().To<CustomEmailService>().InTransientScope();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ninject.Modules;
using RentStuff.Identity.Infrastructure.Services.Email;
using RentStuff.Identity.Infrastructure.Services.Identity;

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
            Bind<IUserTokenProvider<CustomIdentityUser,string>>()
                .To<IUserTokenProvider<CustomIdentityUser,string>>().InTransientScope();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;

namespace RentStuff.Identity.Infrastructure.Persistence.Ninject.Modules
{
    /// <summary>
    /// Ninject Dependency definitions for the IdentityAndAccess.Persistence project
    /// </summary>
    public class IdentityAccessPersistenceNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<IAccountRepository>().To<AccountRepository>().InTransientScope();
        }
    }
}

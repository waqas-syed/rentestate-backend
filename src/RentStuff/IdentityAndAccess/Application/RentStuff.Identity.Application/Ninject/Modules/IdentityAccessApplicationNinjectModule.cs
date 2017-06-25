using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using RentStuff.Identity.Application.Account;

namespace RentStuff.Identity.Application.Ninject.Modules
{
    /// <summary>
    /// Dependencies declaration for IdentityAndAccess.Application project
    /// </summary>
    public class IdentityAccessApplicationNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<IAccountApplicationService>().To<AccountApplicationService>().InTransientScope();
        }
    }
}

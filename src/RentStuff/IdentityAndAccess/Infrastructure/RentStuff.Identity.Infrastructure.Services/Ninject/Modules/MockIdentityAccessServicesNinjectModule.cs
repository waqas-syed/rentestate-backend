using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ninject.Modules;
using RentStuff.Identity.Infrastructure.Services.Email;
using RentStuff.Identity.Infrastructure.Services.Identity;
using RentStuff.Identity.Infrastructure.Services.PasswordReset;

namespace RentStuff.Identity.Infrastructure.Services.Ninject.Modules
{
    /// <summary>
    /// Mock dependencies declaration for the IA.Services project
    /// </summary>
    public class MockIdentityAccessServicesNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<ICustomEmailService>().To<MockCustomEmailService>().InTransientScope();
            // Provide this as a real dependency because it only contains business logic and does no
            // communication any infrastructure
            Bind<IUserTokenProvider<CustomIdentityUser, string>>()
                .To<UserTokenProviderService>().InTransientScope();
        }
    }
}

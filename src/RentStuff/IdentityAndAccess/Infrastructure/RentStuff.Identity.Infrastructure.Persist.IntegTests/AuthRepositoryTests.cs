using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using Spring.Context.Support;

namespace RentStuff.Identity.Infrastructure.Persist.IntegTests
{
    [TestFixture]
    public class AuthRepositoryTests
    {
        [Test]
        public async void RegisterUserTest_TestsIfTheUserIsSavedAsExpectedWhenRegisterMethodIsCalled_VerifiesThroughTheRetruendValueUponRetreival()
        {
            IAuthRepository authRepository = (IAuthRepository)ContextRegistry.GetContext()["AuthRepository"];
            Assert.NotNull(authRepository);
            
            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            CustomIdentityUser customIdentityuser = await authRepository.FindUser(email, password);
            Assert.NotNull(customIdentityuser);

            IdentityResult result = await authRepository.RegisterUser(name, email, password);            
            Assert.IsTrue(result.Succeeded);

            customIdentityuser = await authRepository.FindUser(email, password);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
        }
        
    }
}

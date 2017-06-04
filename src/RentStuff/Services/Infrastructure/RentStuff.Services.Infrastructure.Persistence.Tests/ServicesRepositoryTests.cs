using System;
using System.Reflection;
using System.Reflection.Emit;
using NHibernate;
using NHibernate.Mapping;
using Ninject;
using NUnit.Framework;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernateSession;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.Tests
{
    [TestFixture]
    public class ServicesRepositoryTests
    {
        private readonly ISession session;
        [Test]
        public void ServiceSaveTest_ChecksIfTheServiceInstanceIsSavedAsExpected_VerifiesByTheReturnedValue()
        {
            Service service = new Service("name", "description", "location", "03325329974",
                        "dummy@dumdum123456.com", "Plumber", "Individual", DateTime.Now);
            NHibernateSessionCompound nHibernateSessionCompound = new NHibernateSessionCompound();
            ServicesRepository servicesRepository = new ServicesRepository(nHibernateSessionCompound.SessionFactory);
            servicesRepository.SaveOrUpdate(service);

            var assemblies = new Assembly[] {Assembly.GetAssembly(typeof(ServicesRepository))};
            var kernel = new StandardKernel();
            kernel.Load(assemblies);
            var sessionFactory = kernel.Get<IServicesRepository>();
            
            session.Save(service);
        }
    }
}

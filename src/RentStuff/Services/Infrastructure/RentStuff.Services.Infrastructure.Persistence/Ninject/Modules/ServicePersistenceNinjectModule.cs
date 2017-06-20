using NHibernate;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using RentStuff.Common.NHibernate.Providers;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernate.Providers;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.Ninject.Modules
{
    public class ServicePersistenceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMappingProvider>().To<NhibernateMappingProvider>();
            Bind<IServiceRepository>().To<ServiceRepository>();
        }
    }
}

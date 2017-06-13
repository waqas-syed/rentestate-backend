using NHibernate;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Infrastructure.Persistence.NHibernate.Providers;
using RentStuff.Property.Infrastructure.Persistence.NHibernate.Wrappers;
using RentStuff.Property.Infrastructure.Persistence.Repositories;

namespace RentStuff.Property.Infrastructure.Persistence.Ninject.Modules
{
    public class PropertyPersistenceNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<ISessionFactory>().ToConstant(NHibernateSessionFactoryProvider.SessionFactory).InSingletonScope();
            Bind<INhibernateSessionWrapper>().To<NHibernateSessionWrapper>().InRequestScope();
            Bind<IHouseRepository>().To<HouseRepository>().InTransientScope();
        }
    }
}

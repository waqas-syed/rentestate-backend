using NHibernate;
using Ninject;
using Ninject.Modules;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Infrastructure.Persistence.NHibernate.Providers;
using RentStuff.Property.Infrastructure.Persistence.Repositories;

namespace RentStuff.Property.Infrastructure.Persistence.Ninject.Modules
{
    public class PropertyPersistenceNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<ISessionFactory>().ToConstant(NHibernateSessionFactoryProvider.SessionFactory).InSingletonScope();

            Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession());
            Bind<IHouseRepository>().To<HouseRepository>();
        }
    }
}

using NHibernate;
using Ninject;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Infrastructure.Persistence.NHibernateCompound;
using RentStuff.Property.Infrastructure.Persistence.NHibernateCompound.Providers;
using RentStuff.Property.Infrastructure.Persistence.Repositories;

namespace RentStuff.Property.Infrastructure.Persistence.NinjectCompound
{
    public class PropertyPersistenceNinjectModule : Ninject.Modules.NinjectModule
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

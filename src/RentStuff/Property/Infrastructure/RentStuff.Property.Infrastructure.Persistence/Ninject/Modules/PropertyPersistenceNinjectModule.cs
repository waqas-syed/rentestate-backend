using Ninject.Modules;
using RentStuff.Common.NHibernate.Providers;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.NHibernate.Providers;
using RentStuff.Property.Infrastructure.Persistence.Repositories;

namespace RentStuff.Property.Infrastructure.Persistence.Ninject.Modules
{
    public class PropertyPersistenceNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
           // Bind<IMappingProvider>().To<PropertyNHibernateMapping>();
            //Bind<INHibernateSessionFactoryProvider>().To<NHibernateSessionFactoryProvider>();
            //Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<INHibernateSessionFactoryProvider>().SessionFactory).InSingletonScope();
            //Bind<INhibernateSessionWrapper>().To<NHibernateSessionWrapper>().InRequestScope();
            Bind<IPropertyMappingProvider>().To<PropertyMappingProvider>().InSingletonScope();
            Bind<IResidentialPropertyRepository>().To<PropertyRepository>().InTransientScope();
        }
    }
}

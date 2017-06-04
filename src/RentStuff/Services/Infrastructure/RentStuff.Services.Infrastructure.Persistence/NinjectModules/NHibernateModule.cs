using NHibernate;
using Ninject;
using Ninject.Modules;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernateSession;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.NinjectModules
{
    public class NHibernateModule : NinjectModule
    {
        public override void Load()
        {
            NHibernateSessionCompound nHibernateSessionCompound = new NHibernateSessionCompound();
            Bind<ISessionFactory>().ToConstant(nHibernateSessionCompound.SessionFactory).InSingletonScope();
            Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession());
            Bind<IServicesRepository>().To<ServicesRepository>();
        }
    }
}

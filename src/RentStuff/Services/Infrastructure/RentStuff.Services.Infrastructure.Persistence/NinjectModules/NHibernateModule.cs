using NHibernate;
using Ninject.Modules;
using RentStuff.Services.Infrastructure.Persistence.NHibernateSession;

namespace RentStuff.Services.Infrastructure.Persistence.NinjectModules
{
    public class NHibernateModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<ISessionFactory>().ToProvider<NhibernateSessionFactoryProvider>().InSingletonScope();
            //Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession()).InRequestScope();
        }
    }
}

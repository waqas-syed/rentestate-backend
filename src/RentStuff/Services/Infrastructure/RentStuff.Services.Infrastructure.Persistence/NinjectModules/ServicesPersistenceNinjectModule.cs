using NHibernate;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernateCompound;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.NinjectModules
{
    public class ServicesPersistenceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            NHibernateSessionFactoryProvider nHibernateSessionCompound = new NHibernateSessionFactoryProvider();
            Bind<ISessionFactory>().ToConstant(nHibernateSessionCompound.SessionFactory).InSingletonScope();
            Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession());
            /*.OnActivation(session =>
            {
                session.BeginTransaction();
                session.FlushMode = FlushMode.Commit;
            })
            .OnDeactivation(session =>
            {
                if (session.Transaction.IsActive)
                {
                    try
                    {
                        session.Close();
                        session.Flush();
                        session.Dispose();
                        session.Transaction.Commit();
                    }
                    catch
                    {
                        session.Transaction.Rollback();
                    }
                }
            })*/
            ;
            
            Bind<IServicesRepository>().To<ServicesRepository>();
        }
    }
}

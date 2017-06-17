using NHibernate;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernate.Providers;
using RentStuff.Services.Infrastructure.Persistence.NHibernate.Wrappers;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.Ninject.Modules
{
    public class ServicePersistenceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            NHibernateSessionFactoryProvider nHibernateSessionCompound = new NHibernateSessionFactoryProvider();
            Bind<ISessionFactory>().ToConstant(nHibernateSessionCompound.SessionFactory).InSingletonScope();
            Bind<INhibernateSessionWrapper>().To<NHibernateSessionWrapper>().InRequestScope();
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
            
            Bind<IServiceRepository>().To<ServiceRepository>();
        }
    }
}

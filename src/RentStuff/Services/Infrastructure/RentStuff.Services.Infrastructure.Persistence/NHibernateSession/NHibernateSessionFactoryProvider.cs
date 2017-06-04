using NHibernate;
using Ninject.Activation;

namespace RentStuff.Services.Infrastructure.Persistence.NHibernateSession
{
    public class NhibernateSessionFactoryProvider : Provider<ISessionFactory>
    {
        protected override ISessionFactory CreateInstance(IContext context)
        {
            var sessionFactory = new NHibernateSessionCompound();
            return sessionFactory.GetSessionFactory();
        }
    }
}

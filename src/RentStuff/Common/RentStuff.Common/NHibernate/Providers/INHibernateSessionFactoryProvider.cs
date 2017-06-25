using NHibernate;

namespace RentStuff.Common.NHibernate.Providers
{
    public interface INHibernateSessionFactoryProvider
    {
        ISessionFactory SessionFactory { get; }
    }
}

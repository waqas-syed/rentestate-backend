using NHibernate;

namespace RentStuff.Services.Infrastructure.Persistence.NHibernate.Wrappers
{
    /// <summary>
    /// NhibernateSessionWrapper insterface
    /// </summary>
    public interface INhibernateSessionWrapper
    {
        ISession Session { get; }
    }
}

using NHibernate;

namespace RentStuff.Common.NHibernate.Wrappers
{
    /// <summary>
    /// NhibernateSessionWrapper insterface
    /// </summary>
    public interface INhibernateSessionWrapper
    {
        ISession Session { get; }
    }
}

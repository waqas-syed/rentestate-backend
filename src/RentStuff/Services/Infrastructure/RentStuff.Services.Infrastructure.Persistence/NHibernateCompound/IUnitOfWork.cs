using System;
using NHibernate;

namespace RentStuff.Services.Infrastructure.Persistence.NHibernateCompound
{
    /// <summary>
    /// Interface for IUnitofWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ISession Session { get; }
        void Commit();
        void Rollback();
    }

}

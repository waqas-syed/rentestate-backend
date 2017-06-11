using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace RentStuff.Property.Infrastructure.Persistence.NHibernate.Wrappers
{
    /// <summary>
    /// NhibernateSessionWrapper insterface
    /// </summary>
    public interface INhibernateSessionWrapper
    {
        ISession Session { get; }
    }
}

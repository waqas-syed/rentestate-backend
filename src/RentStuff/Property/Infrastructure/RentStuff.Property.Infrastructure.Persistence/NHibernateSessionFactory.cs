﻿using NHibernate;

namespace RentStuff.Property.Infrastructure.Persistence
{
    /// <summary>
    /// NHibernateSessionFactory
    /// </summary>
    public class NHibernateSessionFactory
    {
        private ISessionFactory _sessionFactory;

        /// <summary>
        /// Session factory for sub-classes.
        /// </summary>
        public ISessionFactory SessionFactory
        {
            protected get { return _sessionFactory; }
            set { _sessionFactory = value;}
        }

        /// <summary>
        /// Get's the current active session. Will retrieve session as managed by the 
        /// Open Session In View module if enabled.
        /// </summary>
        protected ISession CurrentSession
        {
            get
            {
                return _sessionFactory.GetCurrentSession();
            }
        }
    }
}


﻿using NHibernate;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using RentStuff.Common.NHibernate.Providers;
using RentStuff.Common.NHibernate.Wrappers;
using RentStuff.Common.Services.GoogleStorageServices;
using RentStuff.Common.Services.GoogleStorageServices.Mocks;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Common.Services.LocationServices.Mocks;

namespace RentStuff.Common.Ninject.Modules
{
    /// <summary>
    /// Ninject module for loading the Mock dependenciess
    /// </summary>
    public class MockCommonNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<INHibernateSessionFactoryProvider>().To<NHibernateSessionFactoryProvider>().InSingletonScope();
            Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<NHibernateSessionFactoryProvider>().SessionFactory).InSingletonScope();
            //Bind<ISessionFactory>().ToConstant(NHibernateSessionFactoryProvider.SessionFactory).InSingletonScope();
            Bind<INhibernateSessionWrapper>().To<NHibernateSessionWrapper>().InRequestScope();
            Bind<IGeocodingService>().To<MockGeocodingService>().InTransientScope();
            Bind<IPhotoStorageService>().To<MockPhotoStorageService>().InTransientScope();
        }
    }
}

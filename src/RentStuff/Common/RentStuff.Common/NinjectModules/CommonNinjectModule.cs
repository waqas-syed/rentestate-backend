using NHibernate;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using RentStuff.Common.NHibernate.Providers;
using RentStuff.Common.NHibernate.Wrappers;
//using RentStuff.Common.NHibernate.Providers;
//using RentStuff.Common.NHibernate.Wrappers;
using RentStuff.Common.Services.GoogleStorageServices;
using RentStuff.Common.Services.LocationServices;

namespace RentStuff.Common.NinjectModules
{
    /// <summary>
    /// Module for declaring types for Ninject to prepare for Dependency Injection
    /// </summary>
    public class CommonNinjectModule : NinjectModule
    {
        /// <summary>Loads the module into the kernel.</summary>
        public override void Load()
        {
            Bind<INHibernateSessionFactoryProvider>().To<NHibernateSessionFactoryProvider>().InSingletonScope();
            Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<NHibernateSessionFactoryProvider>().SessionFactory).InSingletonScope();
            //Bind<ISessionFactory>().ToConstant(NHibernateSessionFactoryProvider.SessionFactory).InSingletonScope();
            Bind<INhibernateSessionWrapper>().To<NHibernateSessionWrapper>().InRequestScope();
            Bind<IGeocodingService>().To<GeocodingService>().InTransientScope();
            Bind<IPhotoStorageService>().To<PhotoStorageService>().InTransientScope();
        }
    }
}

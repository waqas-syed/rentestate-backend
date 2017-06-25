using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using RentStuff.Common.NHibernate.Providers;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.NHibernate.Providers
{
    /// <summary>
    /// Implementation of the IServiceMappingProvider. This class adds the assemblies that contain the 
    /// Domain model's Nhibernate mapping in the Service Bounded Context
    /// </summary>
    public class ServiceMappingProvider : IServiceMappingProvider
    {
        /// <summary>
        /// The implementation of this method adds the NHibernate mapping assemblies to the given configuration
        /// </summary>
        /// <param name="configuration"></param>
        public void AddMappingAssemblies(Configuration configuration)
        {
            configuration.AddAssembly(typeof(Service).Assembly);
        }
    }
}

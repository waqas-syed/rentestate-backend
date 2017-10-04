using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;
using RentStuff.Common.NHibernate.Providers;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;

namespace RentStuff.Property.Infrastructure.Persistence.NHibernate.Providers
{
    /// <summary>
    /// Implementation of the IServiceMappingProvider. This class adds the assemblies that contain the 
    /// Domain model's Nhibernate mapping in the Service Bounded Context
    /// </summary>
    public class PropertyMappingProvider : IPropertyMappingProvider
    {
        /// <summary>
        /// Adds the mapping assemblies for the Property Bounded Context
        /// </summary>
        /// <param name="cfg"></param>
        public void AddMappingAssemblies(Configuration cfg)
        {
            cfg.AddAssembly(typeof(House).Assembly);
        }
    }
}

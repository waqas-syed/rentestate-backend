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
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Infrastructure.Persistence.Repositories;

namespace RentStuff.Property.Infrastructure.Persistence.NHibernate.Providers
{
    public class PropertyNHibernateMapping : IMappingProvider
    {
        public void GetMappings(Configuration cfg)
        {
            cfg.AddAssembly(typeof(House).Assembly);
            //cfg.AddAssembly(typeof(Dimension).Assembly);
            /* var mapper = new ModelMapper();
 
             mapper.AddMappings(typeof(House).Assembly.GetTypes());
             var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
 
             return mapping;*/
        }
    }
}

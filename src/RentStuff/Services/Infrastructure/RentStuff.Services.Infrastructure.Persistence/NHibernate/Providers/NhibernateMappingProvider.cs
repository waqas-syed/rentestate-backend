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
    public class NhibernateMappingProvider : IMappingProvider
    {
        public void GetMappings(Configuration configuration)
        {
            configuration.AddAssembly(typeof(Service).Assembly);
            configuration.AddAssembly(typeof(ServiceRepository).Assembly);
        }
    }
}

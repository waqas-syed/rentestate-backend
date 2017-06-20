using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

namespace RentStuff.Common.NHibernate.Providers
{
    /// <summary>
    /// Implmentations of this interface will return the assemblies that need to be loaded for NHibernate
    /// </summary>
    public interface IMappingProvider
    {
        void GetMappings(Configuration configuration);
    }
}

using NHibernate.Cfg;

namespace RentStuff.Common.NHibernate.Providers
{
    /// <summary>
    /// Interface specific to Service Bounded Context, so it can implement it and provide the NHibernate 
    /// configuration with its mapping assemblies. 
    /// This interface is the RentStuffCommon project because we create a single NHibernate Session factory
    /// in RentStuff.COmmon and all BCs can use it. As we can't reference othe projects from RentStuff.Common 
    /// as it results in circular dependency, so we use dependency injection for this interface and it's 
    /// implementation
    /// </summary>
    public interface IServiceMappingProvider
    {
        /// <summary>
        /// The implementation of this method adds the NHibernate mapping assemblies to the given configuration
        /// </summary>
        /// <param name="configuration"></param>
        void AddMappingAssemblies(Configuration configuration);
    }
}

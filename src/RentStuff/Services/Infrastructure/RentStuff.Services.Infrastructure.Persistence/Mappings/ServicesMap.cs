using FluentNHibernate.Mapping;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Fluent NHibernate mapping
    /// </summary>
    public class ServicesMap : ClassMap<Service>
    {
        public ServicesMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Location);
            Map(x => x.PhoneNumber).Column("phone_number");
            Map(x => x.Email);
            Map(x => x.ServiceProfessionType).Column("service_profession_type").CustomType(typeof(ServiceProfessionType));
            Map(x => x.ServiceEntityType).Column("service_entity_type").CustomType(typeof(ServiceEntityType));
        }
    }
}

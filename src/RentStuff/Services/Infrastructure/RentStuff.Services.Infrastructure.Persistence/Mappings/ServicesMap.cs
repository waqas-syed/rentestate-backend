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
            Map(x => x.MobileNumber).Column("phone_number");
            Map(x => x.ServiceEmail).Column("service_email");
            Map(x => x.UploaderEmail).Column("uploader_email");
            Map(x => x.ServiceProfessionType).Column("service_profession_type");
            Map(x => x.ServiceEntityType).Column("service_entity_type").CustomType(typeof(ServiceEntityType));
            Map(x => x.Latitude);
            Map(x => x.Longitude);
            Map(x => x.DateEstablished).Column("date_established");
            Map(x => x.SecondaryMobileNumber).Column("secondary_mobile_number");
            Map(x => x.LandlinePhoneNumber).Column("landline_phone_number");
            Map(x => x.Fax).Column("fax");
            HasMany(x => x.Images).Table("serviceimages").KeyColumn("service_id").Element("image_id");
            //HasOne(x => x.Ratings).Cascade.All();
            HasMany(x => x.Reviews).KeyColumn("service_id").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}

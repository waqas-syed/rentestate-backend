using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Fluent NHibernate mapping for the Review class
    /// </summary>
    public class ReviewsMap : ClassMap<Review>
    {
        public ReviewsMap()
        {
            Id(x => x.Id).Column("id");
            Map(x => x.Authorname).Column("author_name");
            Map(x => x.AuthorEmail).Column("author_email");
            Map(x => x.ReviewDescription).Column("review_description");
            References(x => x.Service).Column("service_id");
        }
    }
}

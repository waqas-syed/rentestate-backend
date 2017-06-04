using FluentNHibernate.Mapping;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Maps the Ratings class to the Datbase using Fluent Nhibernate
    /// </summary>
    public class RatingsMap : ClassMap<Ratings>
    {
        public RatingsMap()
        {
            Id(x => x.Id);
            Map(x => x.RatingStars).Column("rating_stars");
            Map(x => x.FiveStarVotes).Column("five_star_votes");
            Map(x => x.FourStarVotes).Column("four_star_votes");
            Map(x => x.ThreeStarVotes).Column("three_star_votes");
            Map(x => x.TwoStarVotes).Column("two_star_votes");
            Map(x => x.OneStarVotes).Column("one_star_votes");
            References(x => x.Service).Unique();
        }
    }
}

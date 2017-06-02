using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RentStuff.Services.Domain.Model.ServicesAggregate;

namespace RentStuff.Services.Domain.Model.UnitTests
{
    [TestFixture]
    class RatingUnitTests
    {
        [Test]
        public void RatingCalculationtest_ChecksIfTheExpectedResultIsAsExpected_VerifiesByTheReturnValue()
        {
            Rating rating = new Rating();
            // Initial Rating is 0
            Assert.AreEqual(0, rating.RatingStars);

            // One rating of 5
            rating.UpdateRatings(5);
            Assert.AreEqual(5, rating.RatingStars);

            // Another Rating of 5
            rating.UpdateRatings(5);
            Assert.AreEqual(5, rating.RatingStars);


            // Lightning struck thrice
            rating.UpdateRatings(5);
            Assert.AreEqual(5, rating.RatingStars);

            // First 4 star rating
            rating.UpdateRatings(4);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(4);
            Assert.AreEqual(4.6, rating.RatingStars);

            rating.UpdateRatings(4);
            Assert.AreEqual(4.5, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.6, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.6, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.7, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.7, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.7, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);

            // It's not possible to go to 5 once a lower rating star has been given
            rating.UpdateRatings(5);
            Assert.AreEqual(4.8, rating.RatingStars);
        }
    }
}

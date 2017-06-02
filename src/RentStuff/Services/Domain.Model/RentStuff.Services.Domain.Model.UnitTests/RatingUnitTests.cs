using NUnit.Framework;
using RentStuff.Services.Domain.Model.ServicesAggregate;

namespace RentStuff.Services.Domain.Model.UnitTests
{
    [TestFixture]
    class RatingUnitTests
    {
        // A few 5 star ratings initially, then a few 4 star ratings, followed by many 5 star ratings
        [Test]
        public void RatingCalculationTest1_ChecksIfTheExpectedResultIsAsExpected_VerifiesByTheReturnValue()
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

            // First 4 star rating. Two more will follow
            rating.UpdateRatings(4);
            Assert.AreEqual(4.8, rating.RatingStars);

            rating.UpdateRatings(4);
            Assert.AreEqual(4.6, rating.RatingStars);

            rating.UpdateRatings(4);
            Assert.AreEqual(4.5, rating.RatingStars);

            // Started getting the 5 star ratings again
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
        
        // Random ratings
        [Test]
        public void RatingCalculationTest2_ChecksIfTheExpectedResultIsAsExpected_VerifiesByTheReturnValue()
        {
            Rating rating = new Rating();
            // Initial Rating is 0
            Assert.AreEqual(0, rating.RatingStars);

            // One rating of 5
            rating.UpdateRatings(5);
            Assert.AreEqual(5, rating.RatingStars);
            
            // First 4 star rating. Two more will follow
            rating.UpdateRatings(1);
            Assert.AreEqual(3, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(3.7, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4, rating.RatingStars);

            // Started getting the 5 star ratings again
            rating.UpdateRatings(5);
            Assert.AreEqual(4.2, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.3, rating.RatingStars);

            rating.UpdateRatings(4);
            Assert.AreEqual(4.3, rating.RatingStars);

            rating.UpdateRatings(4);
            Assert.AreEqual(4.2, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.3, rating.RatingStars);

            rating.UpdateRatings(5);
            Assert.AreEqual(4.4, rating.RatingStars);

            rating.UpdateRatings(2);
            Assert.AreEqual(4.2, rating.RatingStars);

            rating.UpdateRatings(3);
            Assert.AreEqual(4.1, rating.RatingStars);
        }
    }
}

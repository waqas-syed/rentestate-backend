using System;

namespace RentStuff.Services.Domain.Model.ServicesAggregate
{
    /// <summary>
    /// The Rating for a service
    /// </summary>
    public class Rating
    {
        private decimal _rating = 0;
        private decimal _oneStarVotes;
        private decimal _twoStarVotes;
        private decimal _threeStarVotes;
        private decimal _fourStarVotes;
        private decimal _fiveStarVotes;
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Rating()
        {
        }

        /// <summary>
        /// Update the rating by processing the current given value
        /// </summary>
        internal void UpdateRatings(int ratingStars)
        {
            if (ratingStars.Equals(1))
            {
                _oneStarVotes++;
            }
            else if (ratingStars.Equals(2))
            {
                _twoStarVotes++;
            }
            else if (ratingStars.Equals(3))
            {
                _threeStarVotes++;
            }
            else if (ratingStars.Equals(4))
            {
                _fourStarVotes++;
            }
            else if (ratingStars.Equals(5))
            {
                _fiveStarVotes++;
            }
            else
            {
                throw new InvalidOperationException("Only whole integer values from 1-5 can be provided");
            }
            _rating = Math.Round((5*_fiveStarVotes + 4*_fourStarVotes + 3*_threeStarVotes + 2*_twoStarVotes
                            + 1*_oneStarVotes)/(_fiveStarVotes + _fourStarVotes + _threeStarVotes + _twoStarVotes
                                                + _oneStarVotes),1);
        }

        public decimal RatingStars { get { return _rating; } }

        public Service Service { get; set; }

        public decimal FiveStarVotes { get { return _fiveStarVotes; } }

        public decimal FourStarVotes { get { return _fourStarVotes; } }

        public decimal ThreeStarVotes { get { return _threeStarVotes; } }

        public decimal TwoStarVotes { get { return _twoStarVotes; } }

        public decimal OneStarVotes { get { return _oneStarVotes; } }
    }
}

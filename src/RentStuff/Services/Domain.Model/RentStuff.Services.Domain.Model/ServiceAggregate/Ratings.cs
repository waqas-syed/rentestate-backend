using System;

namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// The Rating for a service
    /// </summary>
    public class Ratings
    {
        private string _id = Guid.NewGuid().ToString();
        private decimal _rating = 0;
        private decimal _oneStarVotes;
        private decimal _twoStarVotes;
        private decimal _threeStarVotes;
        private decimal _fourStarVotes;
        private decimal _fiveStarVotes;
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Ratings()
        {
        }

        /// <summary>
        /// Update the rating by processing the current given value
        /// </summary>
        protected internal virtual void UpdateRatings(int ratingStars)
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

        public virtual string Id { get {return _id;} }

        public virtual decimal RatingStars { get { return _rating; } protected internal set { _rating = value; } }

        public virtual Service Service { get; set; }

        public virtual decimal FiveStarVotes { get { return _fiveStarVotes; } protected internal set { _fiveStarVotes = value; } }

        public virtual decimal FourStarVotes { get { return _fourStarVotes; } protected internal set { _fourStarVotes = value; } }

        public virtual decimal ThreeStarVotes { get { return _threeStarVotes; } protected internal set { _threeStarVotes = value; } }

        public virtual decimal TwoStarVotes { get { return _twoStarVotes; } protected internal set { _twoStarVotes = value; } }

        public virtual decimal OneStarVotes { get { return _oneStarVotes; } protected internal set { _oneStarVotes = value; } }
    }
}

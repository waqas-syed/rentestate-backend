using System;
using System.Collections.Generic;
using RentStuff.Common.Domain.Model;

namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// Represents an individual or business in a profression, who can provide it's services to people
    /// </summary>
    public class Service
    {
        private readonly string _id = Guid.NewGuid().ToString();
        private string _name;
        private string _description;
        private string _location;
        private string _phoneNumber;
        private string _email;
        private ServiceProfessionType _serviceProfessionType;
        private ServiceEntityType _serviceEntityType;

        public Service()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the Service class
        /// </summary>
        public Service(string name, string description, string location, string phoneNumber, string email,
            string serviceProviderType, string serviceEntityType, DateTime dateEstablished)
        {
            // Check that the required fields are all provided
            Assertion.AssertStringNotNullorEmpty(name);
            Assertion.AssertStringNotNullorEmpty(location);
            Assertion.AssertStringNotNullorEmpty(phoneNumber);
            Assertion.AssertStringNotNullorEmpty(email);
            Assertion.AssertStringNotNullorEmpty(serviceProviderType);
            Assertion.AssertStringNotNullorEmpty(serviceEntityType);
            Name = name;
            Description = description;
            Location = location;
            PhoneNumber = phoneNumber;
            Email = email;
            SetServiceProfessionType(serviceProviderType);
            SetServiceEntityType(serviceEntityType);
            DateEstablished = dateEstablished;
            Ratings = new Ratings();
            Ratings.Service = this;
            Reviews = new List<Review>();
        }

        /// <summary>
        /// Add a new rating for this service
        /// </summary>
        /// <param name="ratingStars"></param>
        public virtual void AddNewRating(int ratingStars)
        {
            this.Ratings.UpdateRatings(ratingStars);
        }

        /// <summary>
        /// Add a review to this service
        /// </summary>
        /// <param name="authorName"></param>
        /// <param name="authorEmail"></param>
        /// <param name="reviewDescription"></param>
        public virtual void AddReview(string authorName, string authorEmail, string reviewDescription)
        {
            Reviews.Add(new Review(authorName, authorEmail, reviewDescription));
        }

        public virtual string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Name of the business
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Description of the business
        /// </summary>
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Location of the service, either an office or where a service provider spends it's professional time
        /// </summary>
        public virtual string Location
        {
            get { return _location; }
            set
            {
                _location = value;
            }
        }

        /// <summary>
        /// The phone number of the service provider
        /// </summary>
        public virtual string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                Assertion.IsPhoneNumberValid(value);
                _phoneNumber = value;
            }
        }

        /// <summary>
        /// Email of the service provider
        /// </summary>
        public virtual string Email
        {
            get { return _email; }
            set
            {
                Assertion.IsEmailValid(value);
                _email = value;
            }
        }
        
        /// <summary>
        /// Sets the ServiceProviderType by parsing the string and converting it to the expected Enum
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        public virtual void SetServiceProfessionType(string serviceProfessionType)
        {
            _serviceProfessionType = (ServiceProfessionType)Enum.Parse(typeof(ServiceProfessionType), serviceProfessionType);
        }

        /// <summary>
        /// Returns the ServiceProviderType
        /// </summary>
        /// <returns></returns>
        public virtual ServiceProfessionType GetServiceProfessionType()
        {
            return _serviceProfessionType;
        }

        /// <summary>
        /// Sets the ServiceEntityType by parsing the string and converting it to the expected Enum
        /// </summary>
        public virtual void SetServiceEntityType(string serviceEntityType)
        {
            _serviceEntityType = (ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType);
        }

        /// <summary>
        /// Returns the ServiceEntityType
        /// </summary>
        /// <returns></returns>
        public virtual ServiceEntityType GetServiceEntityType()
        {
            return _serviceEntityType;
        }
        
        /// <summary>
        /// When was this service established by the provider
        /// </summary>
        public virtual DateTime DateEstablished { get; set; }

        /// <summary>
        /// Rating of this service by users
        /// </summary>
        public virtual Ratings Ratings { get; set; }

        /// <summary>
        /// Reviews of this service by users
        /// </summary>
        public virtual IList<Review> Reviews { get; set; }

        /// <summary>
        /// Instance builder for the Service class
        /// </summary>
        public class ServiceBuilder
        {
            private string _name;
            private string _description;
            private string _location;
            private string _phoneNumber;
            private string _email;
            private string _serviceProfessionType;
            private string _serviceEntityType;
            private DateTime _dateEstablished;
            private Ratings _rating;
            private IList<Review> _reviews;

            /// <summary>
            /// Name
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public ServiceBuilder Name(string name)
            {
                _name = name;
                return this;
            }

            /// <summary>
            /// Description
            /// </summary>
            /// <param name="description"></param>
            /// <returns></returns>
            public ServiceBuilder Description(string description)
            {
                _description = description;
                return this;
            }

            /// <summary>
            /// Location
            /// </summary>
            /// <param name="location"></param>
            /// <returns></returns>
            public ServiceBuilder Location(string location)
            {
                _location = location;
                return this;
            }

            /// <summary>
            /// PhoneNumber
            /// </summary>
            /// <param name="phoneNumber"></param>
            /// <returns></returns>
            public ServiceBuilder PhoneNumber(string phoneNumber)
            {
                _phoneNumber = phoneNumber;
                return this;
            }

            /// <summary>
            /// Email
            /// </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            public ServiceBuilder Email(string email)
            {
                _email = email;
                return this;
            }

            /// <summary>
            /// ServiceProfessionType
            /// </summary>
            /// <param name="serviceProfessionType"></param>
            /// <returns></returns>
            public ServiceBuilder ServiceProfessionType(string serviceProfessionType)
            {
                _serviceProfessionType = serviceProfessionType;
                return this;
            }

            /// <summary>
            /// ServiceEntityType
            /// </summary>
            /// <param name="serviceEntityType"></param>
            /// <returns></returns>
            public ServiceBuilder ServiceEntityType(string serviceEntityType)
            {
                _serviceEntityType = serviceEntityType;
                return this;
            }

            /// <summary>
            /// DateEstablished
            /// </summary>
            /// <param name="dateEstablished"></param>
            /// <returns></returns>
            public ServiceBuilder DateEstablished(DateTime dateEstablished)
            {
                _dateEstablished = dateEstablished;
                return this;
            }

            /// <summary>
            /// user Ratings
            /// </summary>
            /// <param name="rating"></param>
            /// <returns></returns>
            public ServiceBuilder Rating(Ratings rating)
            {
                _rating = rating;
                return this;
            }

            /// <summary>
            /// User Reviews
            /// </summary>
            /// <param name="reviews"></param>
            /// <returns></returns>
            public ServiceBuilder Reviews(IList<Review> reviews)
            {
                _reviews = reviews;
                return this;
            }

            public Service Build()
            {
                return new Service(_name, _description, _location, _phoneNumber, _email, _serviceProfessionType,
                    _serviceEntityType, _dateEstablished);
            }
        }
    }
}

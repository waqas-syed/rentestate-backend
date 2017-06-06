using System;
using System.Collections.Generic;
using RentStuff.Common.Domain.Model;
// ReSharper disable VirtualMemberCallInConstructor

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
        private string _serviceEmail;
        private string _uploaderEmail;
        private ServiceProfessionType _serviceProfessionType;
        private ServiceEntityType _serviceEntityType;
        private decimal _latitude;
        private decimal _longitude;

        /// <summary>
        /// Default Constructor. To Initialize the Service, use the ServiceBuilder type that uses the Builder
        /// pattern
        /// </summary>
        public Service()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the Service class
        /// </summary>
        private Service(string name, string description, string location, string phoneNumber, 
            string serviceEmail, string uploaderEmail, string serviceProfessionType, string serviceEntityType,
            DateTime dateEstablished, decimal latitude, decimal longitude)
        {
            Name = name;
            Description = description;
            Location = location;
            PhoneNumber = phoneNumber;
            ServiceEmail = serviceEmail;
            UploaderEmail = uploaderEmail;
            SetServiceProfessionType(serviceProfessionType);
            SetServiceEntityType(serviceEntityType);
            DateEstablished = dateEstablished;
            Latitude = latitude;
            Longitude = longitude;
            Ratings = new Ratings();
            Ratings.Service = this;
            Reviews = new List<Review>();
        }

        /// <summary>
        /// Update the details about the service
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="serviceEmail"></param>
        /// <param name="uploaderEmail"></param>
        /// <param name="serviceProfessionType"></param>
        /// <param name="serviceEntityType"></param>
        /// <param name="dateEstablished"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public virtual void UpdateService(string name, string description, string location, string phoneNumber, 
            string serviceEmail, string uploaderEmail, string serviceProfessionType, string serviceEntityType,
            DateTime dateEstablished, decimal latitude, decimal longitude)
        {
            Name = name;
            Description = description;
            Location = location;
            PhoneNumber = phoneNumber;
            ServiceEmail = serviceEmail;
            UploaderEmail = uploaderEmail;
            SetServiceProfessionType(serviceProfessionType);
            SetServiceEntityType(serviceEntityType);
            DateEstablished = dateEstablished;
            Latitude = latitude;
            Longitude = longitude;
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
            Reviews.Add(new Review(authorName, authorEmail, reviewDescription, this));
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
                Assertion.AssertStringNotNullorEmpty(value);
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
                Assertion.AssertStringNotNullorEmpty(value);
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
                Assertion.AssertStringNotNullorEmpty(value);
                Assertion.IsPhoneNumberValid(value);
                _phoneNumber = value;
            }
        }

        /// <summary>
        /// Email of the service provider
        /// </summary>
        public virtual string ServiceEmail
        {
            get { return _serviceEmail; }
            set
            {
                Assertion.IsEmailValid(value);
                _serviceEmail = value;
            }
        }

        /// <summary>
        /// Email of the person who uplaoded this service on our platform. This email will be the user's 
        /// account email
        /// </summary>
        public virtual string UploaderEmail
        {
            get { return _uploaderEmail; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                Assertion.IsEmailValid(value);
                _uploaderEmail = value;
            }
        }

        /// <summary>
        /// Sets the ServiceProviderType by parsing the string and converting it to the expected Enum
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        public virtual void SetServiceProfessionType(string serviceProfessionType)
        {
            Assertion.AssertStringNotNullorEmpty(serviceProfessionType);
            _serviceProfessionType = (ServiceProfessionType)Enum.Parse(typeof(ServiceProfessionType), serviceProfessionType);
        }

        /// <summary>
        /// Gets the ServiceProfessionalType. If you want to set this value, provide a string to the
        /// SetServiceProfessionType method in this class. The reason for a separate method is to accept a 
        /// string and convert it into ServiceProfessionType enum
        /// </summary>
        public virtual ServiceProfessionType ServiceProfessionType
        {
            get { return _serviceProfessionType; }
        }

        /// <summary>
        /// Sets the ServiceEntityType by parsing the string and converting it to the expected Enum
        /// </summary>
        public virtual void SetServiceEntityType(string serviceEntityType)
        {
            Assertion.AssertStringNotNullorEmpty(serviceEntityType);
            _serviceEntityType = (ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType);
        }

        /// <summary>
        /// Gets the ServiceEntityType. If you want to set this value, provide a string to the
        /// SetServiceEntityType method in this class. The reason for a separate method is to accept a 
        /// string and convert it into ServiceEntityType enum
        /// </summary>
        public virtual ServiceEntityType ServiceEntityType
        {
            get { return _serviceEntityType; }
        }

        /// <summary>
        /// When was this service established by the provider
        /// </summary>
        public virtual DateTime DateEstablished { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public virtual decimal Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                Assertion.AssertDecimalNotZero(value);
                _latitude = value;
            }
        }

        /// <summary>
        /// Longitude
        /// </summary>
        public virtual decimal Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                Assertion.AssertDecimalNotZero(value);
                _longitude = value;
            }
        }

        /// <summary>
        /// Rating of this service by users
        /// </summary>
        public virtual Ratings Ratings { get; }

        /// <summary>
        /// Reviews of this service by users
        /// </summary>
        public virtual IList<Review> Reviews { get; }

        /// <summary>
        /// Instance builder for the Service class
        /// </summary>
        public class ServiceBuilder
        {
            private string _name;
            private string _description;
            private string _location;
            private string _phoneNumber;
            private string _serviceEmail;
            private string _uploaderEmail;
            private string _serviceProfessionType;
            private string _serviceEntityType;
            private DateTime _dateEstablished;
            private decimal _latitude;
            private decimal _longitude;

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
            /// Contact Email of the service
            /// </summary>
            /// <param name="serviceEmail"></param>
            /// <returns></returns>
            public ServiceBuilder ServiceEmail(string serviceEmail)
            {
                _serviceEmail = serviceEmail;
                return this;
            }

            /// <summary>
            /// Email of the uploader who uploaded this service to our platform
            /// </summary>
            /// <param name="uploaderEmail"></param>
            /// <returns></returns>
            public ServiceBuilder UploaderEmail(string uploaderEmail)
            {
                _uploaderEmail = uploaderEmail;
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
            /// Latitude
            /// </summary>
            /// <param name="latitude"></param>
            /// <returns></returns>
            public ServiceBuilder Latitude(decimal latitude)
            {
                _latitude = latitude;
                return this;
            }

            /// <summary>
            /// Longitude
            /// </summary>
            /// <param name="longitude"></param>
            /// <returns></returns>
            public ServiceBuilder Longitude(decimal longitude)
            {
                _longitude = longitude;
                return this;
            }

            public Service Build()
            {
                return new Service(_name, _description, _location, _phoneNumber, _serviceEmail, 
                    _uploaderEmail, _serviceProfessionType, _serviceEntityType, _dateEstablished,
                    _latitude, _longitude);
            }
        }
    }
}

using System;
using RentStuff.Common.Domain.Model;

namespace RentStuff.Services.Domain.Model.ServicesAggregate
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
        private ServiceProviderType _serviceProviderType;
        private ServiceEntityType _serviceEntityType;

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
            SetServiceProviderType(serviceProviderType);
            SetServiceEntityType(serviceEntityType);
            DateEstablished = dateEstablished;
        }

        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Name of the business
        /// </summary>
        public string Name
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
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Location of the service, either an office or where a service provider spends it's professional time
        /// </summary>
        public string Location
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
        public string PhoneNumber
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
        public string Email
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
        /// <param name="serviceProviderType"></param>
        public void SetServiceProviderType(string serviceProviderType)
        {
            _serviceProviderType = (ServiceProviderType)Enum.Parse(typeof(ServiceProviderType), serviceProviderType);
        }

        /// <summary>
        /// Returns the ServiceProviderType
        /// </summary>
        /// <returns></returns>
        public ServiceProviderType GetServiceProviderType()
        {
            return _serviceProviderType;
        }

        /// <summary>
        /// Sets the ServiceEntityType by parsing the string and converting it to the expected Enum
        /// </summary>
        public void SetServiceEntityType(string serviceEntityType)
        {
            _serviceEntityType = (ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType);
        }

        /// <summary>
        /// Returns the ServiceEntityType
        /// </summary>
        /// <returns></returns>
        public ServiceEntityType GetServiceEntityType()
        {
            return _serviceEntityType;
        }
        
        /// <summary>
        /// When was this service established by the provider
        /// </summary>
        public DateTime DateEstablished { get; set; }

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
            private string _serviceProviderType;
            private string _serviceEntityType;
            private DateTime _dateEstablished;

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
            /// ServiceProviderType
            /// </summary>
            /// <param name="serviceProviderType"></param>
            /// <returns></returns>
            public ServiceBuilder ServiceProviderType(string serviceProviderType)
            {
                _serviceProviderType = serviceProviderType;
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

            public Service Build()
            {
                return new Service(_name, _description, _location, _phoneNumber, _email, _serviceProviderType,
                    _serviceEntityType, _dateEstablished);
            }
        }
    }
}

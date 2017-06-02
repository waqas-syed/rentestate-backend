using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RentStuff.Services.Domain.Model.ServicesAggregate;

namespace RentStuff.Services.Domain.Model.UnitTests
{
    [TestFixture]
    public class ServiceUnitTests
    {
        [Test]
        public void SuccessfulInstanceCreation_ChecksThatAnInstanceIsCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string email = "smithy@smithereene1234567.com";
            string serviceProviderType = ServiceProviderType.Welder.ToString();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).Email(email)
                .ServiceProviderType(serviceProviderType).ServiceEntityType(serviceEntityType)
                .DateEstablished(dateEstablished).Build();
            Assert.IsNotNull(service);
            Assert.AreEqual(name, service.Name);
            Assert.AreEqual(description, service.Description);
            Assert.AreEqual(location, service.Location);
            Assert.AreEqual(phoneNumber, service.PhoneNumber);
            Assert.AreEqual(email, service.Email);
            Assert.AreEqual((ServiceProviderType)Enum.Parse(typeof(ServiceProviderType), serviceProviderType),
                            service.GetServiceProviderType());
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType),
                            service.GetServiceEntityType());
            Assert.AreEqual(dateEstablished, service.DateEstablished);
        }

        // No name provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoNameFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string email = "smithy@smithereene1234567.com";
            string serviceProviderType = ServiceProviderType.Welder.ToString();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            Service service = new Service.ServiceBuilder().Description(description).Location(location)
                .PhoneNumber(phoneNumber).Email(email).ServiceProviderType(serviceProviderType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished).Build();
        }

        // No Location Provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoLocationFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string phoneNumber = "03455138018";
            string email = "smithy@smithereene1234567.com";
            string serviceProviderType = ServiceProviderType.Welder.ToString();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .PhoneNumber(phoneNumber).Email(email).ServiceProviderType(serviceProviderType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished).Build();
        }

        // No Phone Number Provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoPhoneNumberFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string email = "smithy@smithereene1234567.com";
            string serviceProviderType = ServiceProviderType.Welder.ToString();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).Email(email)
                .ServiceProviderType(serviceProviderType).ServiceEntityType(serviceEntityType)
                .DateEstablished(dateEstablished).Build();
        }

        // No Email Provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoEmailFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceProviderType = ServiceProviderType.Welder.ToString();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber)
                .ServiceProviderType(serviceProviderType).ServiceEntityType(serviceEntityType)
                .DateEstablished(dateEstablished).Build();
        }

        // No ServiceProviderType provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoServiceProviderTypeFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string email = "smithy@smithereene1234567.com";
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).Email(email)
                .ServiceEntityType(serviceEntityType)
                .DateEstablished(dateEstablished).Build();
        }

        // No ServiceEntityType provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoServiceEntityTypeFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string email = "smithy@smithereene1234567.com";
            string serviceProviderType = ServiceProviderType.Welder.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).Email(email)
                .ServiceProviderType(serviceProviderType)
                .DateEstablished(dateEstablished).Build();
        }
    }
}

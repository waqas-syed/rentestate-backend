using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Services.Application.Commands;
using RentStuff.Services.Application.Representations;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Application.ApplicationServices
{
    /// <summary>
    /// ApplicationService for Servies BC
    /// </summary>
    public class ServiceApplicationService : IServiceApplicationService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IServiceRepository _servicesRepository;
        private IGeocodingService _geocodingService;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ServiceApplicationService(IServiceRepository servicesRepository, 
            IGeocodingService geocodingService)
        {
            _servicesRepository = servicesRepository;
            _geocodingService = geocodingService;
        }

        /// <summary>
        /// Save the new given Service
        /// </summary>
        /// <param name="createServiceCommand"></param>
        public void SaveNewService(CreateServiceCommand createServiceCommand)
        {
            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal, decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(
                createServiceCommand.Location);

            if (coordinates == null || coordinates.Item1 == decimal.Zero || coordinates.Item2 == decimal.Zero)
            {
                _logger.Error("Error while getting coordinates for the given address. Address: {0} | " +
                              "UploaderEmail: {1}",
                               createServiceCommand.Location, createServiceCommand.UploaderEmail);
                throw new InvalidDataException($"Could not find coordinates from the given address." +
                                               $" Address: {createServiceCommand.Location} |" +
                                               $" UploaderEmail: {createServiceCommand.UploaderEmail}");
            }

            // Build the Service
            Service service = new Service.ServiceBuilder().Name(createServiceCommand.Name)
                .Description(createServiceCommand.Description)
                .Location(createServiceCommand.Location)
                .PhoneNumber(createServiceCommand.MobileNumber)
                .ServiceEmail(createServiceCommand.ServiceEmail)
                .UploaderEmail(createServiceCommand.UploaderEmail)
                .ServiceProfessionType(createServiceCommand.ServiceProfesionType)
                .ServiceEntityType(createServiceCommand.ServiceEntityType)
                .DateEstablished(createServiceCommand.DateEstablished)
                .Latitude(latitude).Longitude(longitude).SecondaryMobileNumber(secondaryMobileNumber)
                .LandlinePhoneNumber(landlinePhoneNumber).Fax(fax)
                .Build();
            _servicesRepository.SaveOrUpdate();
        }

        /// <summary>
        /// Update the given Service
        /// </summary>
        /// <param name="updateServiceCommand"></param>
        public void UpdateService(UpdateServiceCommand updateServiceCommand)
        {
        }

        public void AddImageToService(string service, IList<string> iamges)
        {
        }

        /// <summary>
        /// Get the Service by Id
        /// </summary>
        /// <param name="id"></param>
        public void GetServiceById(string id)
        {
        }

        /// <summary>
        /// Get the Services given the uploader's email
        /// </summary>
        /// <param name="uploaderEmail"></param>
        public ServicePartialRepresentation GetServicesByUploaderEmail(string uploaderEmail)
        {
            return null;
        }

        /// <summary>
        /// Search the service by the given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="pageNo"></param>
        public ServicePartialRepresentation SearchServicesByLocation(string location, int pageNo = 0)
        {
            return null;
        }

        /// <summary>
        /// Search the service by the given Location and ServiceProfesionType
        /// </summary>
        /// <param name="location"></param>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        public ServicePartialRepresentation SearchServicesByLocationAndProfession(string location, string serviceProfessionType,
            int pageNo = 0)
        {
            return null;
        }

        /// <summary>
        /// Search the service by the given ServiceProfesionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        public ServicePartialRepresentation SearchServicesByProfession(string serviceProfessionType, int pageNo = 0)
        {
            return null;
        }

        /// <summary>
        /// Delete the Service with the given ID
        /// </summary>
        /// <param name="serviceId"></param>
        public void DeleteService(string serviceId)
        {
        }

        /// <summary>
        /// Delete the provided images from the given Service
        /// </summary>
        public void DeleteImagesFromService(string serviceId, List<string> images)
        {
        }
    }
}

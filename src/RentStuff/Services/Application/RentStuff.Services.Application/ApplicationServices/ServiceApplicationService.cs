using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using NLog;
using NLog.Internal;
using RentStuff.Common;
using RentStuff.Common.Services.GoogleStorageServices;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Common.Utilities;
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
        private IPhotoStorageService _photoStorageService;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ServiceApplicationService(IServiceRepository servicesRepository,
            IGeocodingService geocodingService, IPhotoStorageService photoStorageService)
        {
            _servicesRepository = servicesRepository;
            _geocodingService = geocodingService;
            _photoStorageService = photoStorageService;
        }

        /// <summary>
        /// Save the new given Service
        /// </summary>
        /// <param name="createServiceCommand"></param>
        public string SaveNewService(CreateServiceCommand createServiceCommand)
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
            Service service = new Service.ServiceBuilder()
                .Name(createServiceCommand.Name)
                .Description(createServiceCommand.Description)
                .Location(createServiceCommand.Location)
                .PhoneNumber(createServiceCommand.MobileNumber)
                .ServiceEmail(createServiceCommand.ServiceEmail)
                .UploaderEmail(createServiceCommand.UploaderEmail)
                .ServiceProfessionType(createServiceCommand.ServiceProfesionType)
                .ServiceEntityType(createServiceCommand.ServiceEntityType)
                .DateEstablished(createServiceCommand.DateEstablished)
                .Latitude(coordinates.Item1)
                .Longitude(coordinates.Item2)
                .FacebookLink(createServiceCommand.FacebookLink)
                .TwitterLink(createServiceCommand.TwitterLink)
                .InstagramLink(createServiceCommand.InstagramLink)
                .WebsiteLink(createServiceCommand.WebsiteLink)
                .Build();
            return _servicesRepository.SaveOrUpdate(service);
        }

        /// <summary>
        /// Update the given Service
        /// </summary>
        /// <param name="updateServiceCommand"></param>
        public void UpdateService(UpdateServiceCommand updateServiceCommand)
        {
            if (updateServiceCommand == null)
            {
                throw new NullReferenceException("UpdateServiceCommand is null");
            }

            // Retrieve the servie and check if it is null
            var service = _servicesRepository.GetServiceById(updateServiceCommand.Id);
            if (service == null)
            {
                _logger.Error("No Service found for the given ServiceId: {0}", updateServiceCommand.Id);
                throw new NullReferenceException("No Service found for the given ServiceId: "
                                                 + updateServiceCommand.Id);
            }

            Tuple<decimal, decimal> updatedCoordinates;
            // If the location of the Service is changed, then change the coordinates as well
            if (!service.Location.Equals(updateServiceCommand.Location))
            {
                updatedCoordinates =
                    _geocodingService.GetCoordinatesFromAddress(updateServiceCommand.Location);
            }
            else
            {
                updatedCoordinates = new Tuple<decimal, decimal>(service.Latitude, service.Longitude);
            }
            service.UpdateService(updateServiceCommand.Name, updateServiceCommand.Description,
                updateServiceCommand.Location, updateServiceCommand.MobileNumber,
                updateServiceCommand.ServiceEmail, updateServiceCommand.UploaderEmail,
                updateServiceCommand.ServiceProfesionType, updateServiceCommand.ServiceEntityType,
                updateServiceCommand.DateEstablished, updatedCoordinates.Item1, updatedCoordinates.Item2,
                updateServiceCommand.FacebookLink, updateServiceCommand.InstagramLink,
                updateServiceCommand.TwitterLink, updateServiceCommand.WebsiteLink);
            _servicesRepository.SaveOrUpdate(service);
        }

        public void AddImageToService(string service, IList<string> iamges)
        {
        }

        /// <summary>
        /// Get the Service by Id
        /// </summary>
        /// <param name="id"></param>
        public ServiceFullRepresentation GetServiceById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NullReferenceException("The ServiceId for retrieving Service is empty");
            }
            var service = _servicesRepository.GetServiceById(id);
            if (service == null)
            {
                _logger.Error("No Service found for ServiceId: {0}", id);
                throw new NullReferenceException($"No Service found for ServiceId{id}");
            }
            return ConvertSingleServiceToFullRepresentation(service);
        }

        /// <summary>
        /// Get the Services given the uploader's email
        /// </summary>
        /// <param name="uploaderEmail"></param>
        /// <param name="pageNo"></param>
        public IList<ServicePartialRepresentation> GetServicesByUploaderEmail(string uploaderEmail, 
            int pageNo = 0)
        {
            var retrievedServices = _servicesRepository.GetServicesByEmail(uploaderEmail, pageNo);
            return ConvertServicesToPartialRepresentations(retrievedServices);
        }

        /// <summary>
        /// Search the service by the given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="pageNo"></param>
        public IList<ServicePartialRepresentation> SearchServicesByLocation(string location, int pageNo = 0)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(location);
            var retrievedServices = _servicesRepository.GetServicesByLocation(coordinates.Item1,
                coordinates.Item2, pageNo);
            return ConvertServicesToPartialRepresentations(retrievedServices);
        }

        /// <summary>
        /// Search the service by the given Location and ServiceProfesionType
        /// </summary>
        /// <param name="location"></param>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        public IList<ServicePartialRepresentation> SearchServicesByLocationAndProfession(string location,
            string serviceProfessionType, int pageNo = 0)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(location);
            var retrievedServices = _servicesRepository.GetServicesByLocationAndProfession(coordinates.Item1,
                coordinates.Item2, serviceProfessionType, pageNo);
            return ConvertServicesToPartialRepresentations(retrievedServices);
        }

        /// <summary>
        /// Search the service by the given ServiceProfesionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        public IList<ServicePartialRepresentation> SearchServicesByProfession(string serviceProfessionType,
            int pageNo = 0)
        {
            var retrievedServices = _servicesRepository.GetServicesByProfession(
                serviceProfessionType, pageNo);
            return ConvertServicesToPartialRepresentations(retrievedServices);
        }

        /// <summary>
        /// Gets all the services present in the database
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<ServicePartialRepresentation> GetAllServices(int pageNo = 0)
        {
            var retrievedServices = _servicesRepository.GetAllServices(pageNo);
            return ConvertServicesToPartialRepresentations(retrievedServices);
        }

        /// <summary>
        /// Add an image to this service
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="photoStream"></param>
        public void AddSingleImageToService(string serviceId, Stream photoStream)
        {
            using (var image = Image.FromStream(photoStream))
            {
                // Get the house from the repository
                Service service = _servicesRepository.GetServiceById(serviceId);
                // If we find a hosue with the given ID
                if (service != null)
                {
                    // Create a name for this image
                    string imageId = "IMG_" + Guid.NewGuid().ToString();
                    // Add extension to the file name
                    String fileName = imageId + ImageFurnace.GetImageExtension();
                    // Resize the image to the size that we will be using as default
                    var finalImage = ImageFurnace.ResizeImage(image, 830, 500);
                    // Get the stream of the image
                    var httpPostedStream = ImageFurnace.ToStream(finalImage);
                    _photoStorageService.UploadPhoto(fileName, httpPostedStream);
                    // Get the url of the bucket and append with it the name of the file. This will be the public 
                    // url for this image and ready to view
                    fileName = System.Configuration.ConfigurationManager.AppSettings["GoogleCloudStoragePhotoBucketUrl"] + fileName;
                    // Add the image link to the list of images this house owns
                    service.AddImage(fileName);
                    // Save the updated house in the repository
                    _servicesRepository.SaveOrUpdate(service);
                    // Log the info
                    _logger.Info("Added images to house successfully. HouseId: {0}", service.Id);
                }
                // Otherwise throw an exception
                else
                {
                    throw new NullReferenceException("No house found with the given ID");
                }
            }
        }

        /// <summary>
        /// Delete the Service with the given ID
        /// </summary>
        /// <param name="serviceId"></param>
        public void DeleteService(string serviceId)
        {
            // Check if Service id is not null
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                _logger.Error("ServiceId to delete Service is empty");
                throw new NullReferenceException("ServiceId to delete Service is empty");
            }
            // Get the service
            var service = _servicesRepository.GetServiceById(serviceId);
            // Check if the service is found or not
            if (service == null)
            {
                _logger.Error("No Service found for ServiceId: {0}", serviceId);
                throw new NullReferenceException($"No Service found for ServiceId{serviceId}");
            }
            _servicesRepository.DeleteService(service);
        }

        /// <summary>
        /// Delete the provided images from the given Service
        /// </summary>
        public void DeleteImagesFromService(string serviceId, List<string> images)
        {
            // Get the service
            var service = GetServiceByIdUsingChecks(serviceId);

            foreach (var image in images)
            {
                bool imageRemoved = service.Images.Remove(image);
                if (imageRemoved)
                {
                    try
                    {
                        _photoStorageService.DeletePhoto(image);
                    }
                    catch (Exception)
                    {
                        _logger.Error("GOOGLE CLOUD: Coud not delete image. ServiceId: {0} | ImageLink : {1} " +
                                      "| UploaderEmail: {2}", serviceId, image, service.UploaderEmail);
                    }
                }
            }
            _servicesRepository.SaveOrUpdate(service);
        }

        #region Private Methods

        /// <summary>
        /// Abstract method for getting a service by the given Id. Asserts multiple checks
        /// </summary>
        /// <param name="serviceId"></param>
        private Service GetServiceByIdUsingChecks(string serviceId)
        {
            // Check if Service id is not null
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                _logger.Error("ServiceId to delete Service is empty");
                throw new NullReferenceException("ServiceId to delete Service is empty");
            }
            // Get the service
            var service = _servicesRepository.GetServiceById(serviceId);
            // Check if the service is found or not
            if (service == null)
            {
                _logger.Error("No Service found for ServiceId: {0}", serviceId);
                throw new NullReferenceException($"No Service found for ServiceId{serviceId}");
            }
            return service;
        }

        /// <summary>
        /// Converts a single service a ServiceFullRepresentation
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private ServiceFullRepresentation ConvertSingleServiceToFullRepresentation(Service service)
        {
            return new ServiceFullRepresentation(service.Id, service.Name, service.Description,
                service.Location, service.MobileNumber, service.ServiceEmail, service.ServiceProfessionType,
                service.ServiceEntityType.ToString(), service.FacebookLink, service.InstagramLink, service.TwitterLink,
                service.WebsiteLink, service.DateEstablished, 
                // Provide the images and review as a read-only collection
                new ReadOnlyCollection<string>(service.Images),
                new ReadOnlyCollection<Review>(service.Reviews));
        }

        /// <summary>
        /// Converts the given set of services to corresponding Partial Services instances
        /// </summary>
        /// <param name="serviceList"></param>
        /// <returns></returns>
        private IList<ServicePartialRepresentation> ConvertServicesToPartialRepresentations(IList<Service>
            serviceList)
        {
            IList<ServicePartialRepresentation> partialServiceList = new List<ServicePartialRepresentation>();
            foreach (var service in serviceList)
            {
                string defaultImageLink = null;
                if (service.Images.Count > 0)
                {
                    defaultImageLink = service.Images.First();
                }
                var servicePartialRepresentation = new ServicePartialRepresentation(service.Name,
                    service.Location, service.MobileNumber, service.ServiceEmail, 
                    service.ServiceProfessionType, service.ServiceEntityType.ToString(), service.FacebookLink,
                    service.InstagramLink, service.TwitterLink, service.WebsiteLink, defaultImageLink);
                partialServiceList.Add(servicePartialRepresentation);
            }
            return partialServiceList;
        }
        
        #endregion Private Methods
    }
}

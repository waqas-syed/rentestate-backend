using Newtonsoft.Json;
using NLog;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.PropertyServices.Commands.CreateCommands;
using RentStuff.Property.Application.PropertyServices.Representation;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;
using RentStuff.Property.Application.PropertyServices.Representation.FullRepresentations;
using RentStuff.Property.Application.PropertyServices.Representation.PartialRepresentations;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HotelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;
using System.Threading;
using RentStuff.Property.Application.PropertyServices.Commands.UpdateCommands;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace RentStuff.Property.Application.PropertyServices
{
    /// <summary>
    /// House Application Service
    /// </summary>
    public class PropertyApplicationService : IPropertyApplicationService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IResidentialPropertyRepository _residentialPropertyRepository;
        private RentStuff.Common.Services.LocationServices.IGeocodingService _geocodingService;
        private RentStuff.Common.Services.GoogleStorageServices.IPhotoStorageService _photoStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PropertyApplicationService(IResidentialPropertyRepository houseRepository,
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService,
            RentStuff.Common.Services.GoogleStorageServices.IPhotoStorageService photoStorageService)
        {
            _residentialPropertyRepository = houseRepository;
            _geocodingService = geocodingService;
            _photoStorageService = photoStorageService;
        }

        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        public string SaveNewProperty(object propertyJson, string currentUserEmail)
        {
            var propertyBaseCommand = CreateUpdateRequiredFieldsCheck(propertyJson, currentUserEmail);

            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal, decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(propertyBaseCommand.Area.Value);
            var genderRestriction = ParseGenderRestriction(propertyBaseCommand.GenderRestriction.Value);
            
            // The Id of whichever property gets created
            string id = null;

            // Now check what type of command it is, and cast it to that property type
            if (propertyBaseCommand.PropertyType.Value.Equals(Constants.House) ||
                propertyBaseCommand.PropertyType.Value.Equals(Constants.Apartment))
            {
                CreateHouseCommand createHouseCommand =
                    JsonConvert.DeserializeObject<CreateHouseCommand>(propertyBaseCommand.ToString());

                House house = CreateHouseInstance(createHouseCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
                // Save the new house instance
                _residentialPropertyRepository.SaveorUpdate(house);
                _logger.Info("House uploaded Successfully: {0}", house);
                return house.Id;
            }
            throw new NotImplementedException("Only residential Property types are supported yet");
            // If the request is for creating a new Hostel
            /*else if (propertyBaseCommand.PropertyType.Value.Equals(Constants.Hostel))
            {
                CreateHostelCommand createHostelCommand = 
                    JsonConvert.DeserializeObject<CreateHostelCommand>(propertyBaseCommand.ToString());
                Hostel hostel = CreateHostelInstance(createHostelCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
                _residentialPropertyRepository.SaveorUpdate(hostel);
                _logger.Info("Hostel uploaded Successfully: {0}", hostel);

                id = hostel.Id;
            }
            // If the request is for creating a new Hotel Guest House
            else if (propertyBaseCommand.PropertyType.Value.Equals(Constants.Hotel) ||
                     propertyBaseCommand.PropertyType.Value.Equals(Constants.GuestHouse))
            {
                CreateHotelCommand createHotelCommand = 
                    JsonConvert.DeserializeObject<CreateHotelCommand>(propertyBaseCommand.ToString());
                Hotel hotel = CreateHotelInstance(createHotelCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
                _residentialPropertyRepository.SaveorUpdate(hotel);
                _logger.Info("Hotel uploaded Successfully: {0}", hotel);

                id = hotel.Id;
            }
            else
            {
                
            }*/
        }

        /// <summary>
        /// Checks that the fields necessary for Creating new property and updating the property are provided
        /// Throws exception otherwise
        /// Returns the base type for all properties after parsing it from the provided JSON
        /// </summary>
        private dynamic CreateUpdateRequiredFieldsCheck(object propertyJson, string currentUserEmail)
        {
            dynamic propertyBaseCommand = JsonConvert.DeserializeObject<dynamic>(propertyJson.ToString());

            // Check if the owner and current user's email are not empty
            if (string.IsNullOrWhiteSpace(propertyBaseCommand.OwnerEmail.Value))
            {
                throw new NullReferenceException("Property Owner Email must be provided");
            }
            if (string.IsNullOrWhiteSpace(currentUserEmail))
            {
                throw new NullReferenceException("Current user's Email must be provided");
            }
            // Check that the current user is posting the property on his own email as the owner
            CheckOwnerEmailIntegrity(currentUserEmail, propertyBaseCommand.OwnerEmail.Value);

            // Check other mandatory values before proceeding
            if (string.IsNullOrWhiteSpace(propertyBaseCommand.PropertyType.Value))
            {
                throw new NullReferenceException("PropertyType must be provided");
            }
            if (string.IsNullOrWhiteSpace(propertyBaseCommand.Area.Value))
            {
                throw new NullReferenceException("Area must be provided");
            }
            return propertyBaseCommand;
        }

        /// <summary>
        /// Parse the Gender Restriction string to the actual enum value
        /// </summary>
        /// <param name="genderRestrictionString"></param>
        private GenderRestriction ParseGenderRestriction(string genderRestrictionString)
        {
            GenderRestriction genderRestriction = default(GenderRestriction);

            if (!string.IsNullOrWhiteSpace(genderRestrictionString))
            {
                Enum.TryParse(genderRestrictionString, out genderRestriction);
            }
            return genderRestriction;
        }

        /// <summary>
        /// Updates an exisitng house
        /// </summary>
        /// <param name="propertyJson"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        public void UpdateProperty(object propertyJson, string currentUserEmail)
        {
            var propertyBaseCommand = CreateUpdateRequiredFieldsCheck(propertyJson, currentUserEmail);

            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal, decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(propertyBaseCommand.Area.Value);
            var genderRestriction = ParseGenderRestriction(propertyBaseCommand.GenderRestriction.Value);
            if (coordinates == null || coordinates.Item1 == decimal.Zero || coordinates.Item2 == decimal.Zero)
            {
                throw new InvalidDataException(
                    $"Could not find coordinates from the given address: {propertyBaseCommand.Area.Value}");
            }
            // Now check what type of command it is, and cast it to that property type
            // If House or apartment is requested
            if (propertyBaseCommand.PropertyType.Value.Equals(Constants.House) ||
                propertyBaseCommand.PropertyType.Value.Equals(Constants.Apartment))
            {
                UpdateHouseCommand updateHouseCommand =
                    JsonConvert.DeserializeObject<UpdateHouseCommand>(propertyBaseCommand.ToString());

                UpdateHouse(updateHouseCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
            }
            // If Hostel update is requested
            else if (propertyBaseCommand.PropertyType.Value.Equals(Constants.Hostel))
            {
                UpdateHostelCommand updateHostelCommand =
                    JsonConvert.DeserializeObject<UpdateHostelCommand>(propertyBaseCommand.ToString());
                UpdateHostel(updateHostelCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
            }
            // If Hotel or Guest House update is requested
            else if (propertyBaseCommand.PropertyType.Value.Equals(Constants.Hotel) ||
                     propertyBaseCommand.PropertyType.Value.Equals(Constants.GuestHouse))
            {
                UpdateHotelCommand updateHotelCommand =
                    JsonConvert.DeserializeObject<UpdateHotelCommand>(propertyBaseCommand.ToString());
                UpdateHotel(updateHotelCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
            }
            else
            {
                throw new NotImplementedException("Requested Property Type not supported");
            }
        }

        /// <summary>
        /// Delete the given house instance
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="currentUserEmail"></param>
        public void DeleteHouse(string houseId, string currentUserEmail)
        {
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_residentialPropertyRepository.GetPropertyById(houseId);
            if (property != null)
            {
                // Check that the current user is posting the property on his own email as the owner
                CheckOwnerEmailIntegrity(property.OwnerEmail, currentUserEmail);
                // Delete all the images from the Google cloud storage photo bucket
                foreach (var image in property.Images)
                {
                    _photoStorageService.DeletePhoto(image);
                }
                _residentialPropertyRepository.Delete(property);
                _logger.Info("Deleted property successfully: {0}", property);
            }
            else
            {
                throw new InstanceNotFoundException($"No property could be found for the given id. HouseId: {houseId}");
            }
        }

        /// <summary>
        /// Get all of the properties saved in the database
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<ResidentialPropertyPartialBaseImplementation> GetAllProperties(int pageNo = 0)
        {
            IList<Domain.Model.PropertyAggregate.Property> properties =
                 _residentialPropertyRepository.GetAllProperties(pageNo);

            return ConvertPropertiesToPartialRepresentation(properties);
        }

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        public IList<ResidentialPropertyPartialBaseImplementation> GetPropertiesByEmail(string email,
            int pageNo = 0)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new NullReferenceException("Email cannot be null");
            }
            var properties = _residentialPropertyRepository.GetHouseByOwnerEmail(email, pageNo);
            return ConvertPropertiesToPartialRepresentation(properties);
            /*switch (propertyType)
            {
                case Constants.House:
                    var houses = _residentialPropertyRepository.GetHouseByOwnerEmail(email, pageNo);
                    return ConvertHousesToPartialRepresentations(houses);
                case Constants.Apartment:
                    var apartments = _residentialPropertyRepository.GetApartmentByOwnerEmail(email, pageNo);
                    return ConvertHousesToPartialRepresentations(apartments);
                case Constants.Hostel:
                    var hostels = _residentialPropertyRepository.GetHostelsByOwnerEmail(email, pageNo);
                    return ConvertHostelsToPartialRepresentations(hostels);
                case Constants.Hotel:
                    var hotels = _residentialPropertyRepository.GetHotelsByOwnerEmail(email, pageNo);
                    return ConvertHotelsToPartialRepresentations(hotels);
                case Constants.GuestHouse:
                    var guestHouses = _residentialPropertyRepository.GetGuestHousesByOwnerEmail(email, pageNo);
                    return ConvertHotelsToPartialRepresentations(guestHouses);
                default:
                    throw new NotImplementedException("Requested Property type is not supported");
            }*/
        }

        /// <summary>
        /// Get House by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public PropertyBaseRepresentation GetPropertyById(string id, string propertyType)
        {
            switch (propertyType)
            {
                case Constants.House:
                    return ConvertHouseToRepresentation(id);

                case Constants.Apartment:
                    return ConvertHouseToRepresentation(id);

                case Constants.Hostel:
                    return ConvertHostelToRepresentation(id);

                case Constants.Hotel:
                    return ConvertHotelToRepresentation(id);

                case Constants.GuestHouse:
                    return ConvertHotelToRepresentation(id);

                default:
                    throw new NotImplementedException("Request property type is not supported yet");
            }
        }
        
        /// <summary>
        /// Search houses with reference to propertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        /*public IList<ResidentialPropertyPartialBaseImplementation> SearchPropertiesByPropertyType(string propertyType, int pageNo = 0)
        {
            switch (propertyType)
            {
                case Constants.House:
                    var houses = _residentialPropertyRepository.GetAllHouses(pageNo);
                    return ConvertHousesToPartialRepresentations(houses);
                case Constants.Apartment:
                    var apartments = _residentialPropertyRepository.GetAllApartments(pageNo);
                    return ConvertHousesToPartialRepresentations(apartments);
                case Constants.Hostel:
                    var hostels = _residentialPropertyRepository.GetAllHostels(pageNo);
                    return ConvertHostelsToPartialRepresentations(hostels);
                case Constants.Hotel:
                    var hotels = _residentialPropertyRepository.GetAllHotels(pageNo);
                    return ConvertHotelsToPartialRepresentations(hotels);
                case Constants.GuestHouse:
                    var guestHouses = _residentialPropertyRepository.GetAllGuestHouses(pageNo);
                    return ConvertHotelsToPartialRepresentations(guestHouses);
                default:
                    throw new NotImplementedException("Requested Proeprty type is not supported");
            }
        }*/

        /// <summary>
        /// Search nearby houses by providing the area and property type
        /// </summary>
        /// <param name="address"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<ResidentialPropertyPartialBaseImplementation> SearchPropertiesByArea(
            string address, int pageNo = 0)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);

            var houses = _residentialPropertyRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
            return ConvertHousesToPartialRepresentations(houses);
            /*switch (propertyType)
            {
                case Constants.House:
                    
                case Constants.Apartment:
                    var apartments = _residentialPropertyRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2, Constants.Apartment, pageNo);
                    return ConvertHousesToPartialRepresentations(apartments);
                case Constants.Hostel:
                    var hostels = _residentialPropertyRepository.SearchHostelByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    return ConvertHostelsToPartialRepresentations(hostels);
                case Constants.Hotel:
                    var hotels = _residentialPropertyRepository.SearchHotelByCoordinates(coordinates.Item1, coordinates.Item2, Constants.Hotel, pageNo);
                    return ConvertHotelsToPartialRepresentations(hotels);
                case Constants.GuestHouse:
                    var guestHouses = _residentialPropertyRepository.SearchHotelByCoordinates(coordinates.Item1, coordinates.Item2, Constants.GuestHouse, pageNo);
                    return ConvertHotelsToPartialRepresentations(guestHouses);
            }
            throw new NotImplementedException("Requested property Type is not supported");*/
        }

        /// <summary>
        /// Get the number of records in the database for the given criteria
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="location"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public HouseCountRepresentation GetRecordsCount(string propertyType, string location, string email)
        {
            Tuple<int, int> recordCount = null;
            // If location is not null
            if (!string.IsNullOrWhiteSpace(location))
            {
                // Get the coordinates for the location using the Geocoding API service
                var coordinates = _geocodingService.GetCoordinatesFromAddress(location);
                recordCount = _residentialPropertyRepository.GetRecordCountByLocation(coordinates.Item1, coordinates.Item2);
                return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
                
                /*
                // And property type is also not null
                if (!string.IsNullOrWhiteSpace(propertyType))
                {
                    recordCount = _residentialPropertyRepository.GetRecordCountByLocationAndPropertyType(coordinates.Item1,
                        coordinates.Item2,
                        propertyType);
                    return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
                }
                // otherwise just get the count for houses given the coordinates
                else
                {
                    recordCount = _residentialPropertyRepository.GetRecordCountByLocation(coordinates.Item1, coordinates.Item2);
                    return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
                }*/
            }
            /*if (!string.IsNullOrWhiteSpace(propertyType))
            {
                recordCount = _residentialPropertyRepository.GetRecordCountByPropertyType(propertyType);
                return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
            }*/
            if (!string.IsNullOrWhiteSpace(email))
            {
                recordCount = _residentialPropertyRepository.GetRecordCountByEmail(email);
                return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
            }
            // If no criteria is given, return the total number of houses present in the database
            recordCount = _residentialPropertyRepository.GetTotalRecordCount();
            return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
        }
        
        /// <summary>
        /// Get the types of peroperty: Apartment, House, Hostel, Room
        /// </summary>
        /// <returns></returns>
        public IList<string> GetPropertyTypes()
        {
            return House.GetAllPropertyTypes();
        }

        /// <summary>
        /// Adds a single image to a house
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="photoStream"></param>
        public void AddSingleImageToHouse(string houseId, Stream photoStream)
        {
            // Get the house from the repository
            Property.Domain.Model.PropertyAggregate.Property property= _residentialPropertyRepository.GetPropertyById(houseId);

            // If we find a house with the given ID
            if (property != null)
            {
                // Create a name for this image
                string imageId = "IMG_" + Guid.NewGuid().ToString();
                // Add extension to the file name
                String fileName = imageId + ImageFurnace.GetImageExtension();
                // Resize the image to the size that we will be using as default
                //var finalImage = ImageFurnace.ResizeImage(image, 830, 500);
                // Get the stream of the image
                //var httpPostedStream = ImageFurnace.ToStream(finalImage);
                _photoStorageService.UploadPhoto(fileName, photoStream);
                // Get the url of the bucket and append with it the name of the file. This will be the public 
                // url for this image and ready to view
                fileName = ConfigurationManager.AppSettings["GoogleCloudStoragePhotoBucketUrl"] + fileName;
                // Add the image link to the list of images this house owns
                property.AddImage(fileName);
                // Save the updated house in the repository
                _residentialPropertyRepository.SaveorUpdate(property);
                // Log the info
                _logger.Info("Added images to house successfully. HouseId: {0}", property.Id);
            }
            // Otherwise throw an exception
            else
            {
                throw new NullReferenceException("No house found with the given ID");
            }
        }

        /// <summary>
        /// Delete the images for the house
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="imagesList"></param>
        /// <returns></returns>
        public void DeleteImagesFromHouse(string houseId, IList<string> imagesList)
        {
            var house = (ResidentialProperty)_residentialPropertyRepository.GetPropertyById(houseId);
            if (house != null && imagesList.Count > 0)
            {
                foreach (var imageId in imagesList)
                {
                    house.Images.Remove(imageId);
                    try
                    {
                        _photoStorageService.DeletePhoto(imageId);
                    }
                    catch (Exception)
                    {
                        // Do nothing. Even if it is not deleted from Google Cloud, just delete it from our 
                        // database
                    }
                }
                _residentialPropertyRepository.SaveorUpdate(house);
                _logger.Info("Deleted images from house successfully. HouseId: {0}", house.Id);
            }
        }

        /// <summary>
        /// Check that the given email is the same as the owner of the given house id
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="requesterEmail"></param>
        /// <returns></returns>
        public bool HouseOwnershipCheck(string propertyId, string requesterEmail)
        {
            var house = (ResidentialProperty)_residentialPropertyRepository.GetPropertyById(propertyId);
            if (house == null)
            {
                throw new NullReferenceException($"No property found for the given Id: {propertyId}");
            }
            if (house.OwnerEmail.Equals(requesterEmail, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the lsit of Rent Units
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllRentUnits()
        {
            return House.GetAllRentUnits();
        }
        
        #region Create Property Helper Methods

        /// <summary>
        /// Creates a new House instance and returns it
        /// </summary>
        /// <param name="createHouseCommand"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="genderRestriction"></param>
        private House CreateHouseInstance(CreateHouseCommand createHouseCommand, decimal latitude, decimal longitude,
            GenderRestriction genderRestriction)
        {
            // Create the new house instance
            House house = new House.HouseBuilder()
                .Title(createHouseCommand.Title)
                .Description(createHouseCommand.Description)
                .CableTvAvailable(createHouseCommand.CableTvAvailable)
                .GarageAvailable(createHouseCommand.GarageAvailable)
                .LandlinePhoneAvailable(createHouseCommand.LandlinePhoneAvailable)
                .RentPrice(createHouseCommand.RentPrice)
                .NumberOfBathrooms(createHouseCommand.NumberOfBathrooms)
                .NumberOfBedrooms(createHouseCommand.NumberOfBedrooms)
                .NumberOfKitchens(createHouseCommand.NumberOfKitchens)
                .OwnerEmail(createHouseCommand.OwnerEmail.ToLower())
                .OwnerPhoneNumber(createHouseCommand.OwnerPhoneNumber)
                .SmokingAllowed(createHouseCommand.SmokingAllowed)
                .WithInternetAvailable(createHouseCommand.InternetAvailable)
                .PropertyType(createHouseCommand.PropertyType)
                .Latitude(latitude)
                .Longitude(longitude)
                .HouseNo(createHouseCommand.HouseNo)
                .StreetNo(createHouseCommand.StreetNo)
                .Area(createHouseCommand.Area)
                .OwnerName(createHouseCommand.OwnerName)
                .GenderRestriction(genderRestriction)
                .IsShared(createHouseCommand.IsShared)
                .RentUnit(createHouseCommand.RentUnit)
                .LandlineNumber(createHouseCommand.LandlineNumber)
                .Fax(createHouseCommand.Fax)
                .Bathtub(createHouseCommand.Bathtub)
                .AC(createHouseCommand.AC)
                .Geyser(createHouseCommand.Geyser)
                .Balcony(createHouseCommand.Balcony)
                .Lawn(createHouseCommand.Lawn)
                .CctvCameras(createHouseCommand.CctvCameras)
                .BackupElectricity(createHouseCommand.BackupElectricity)
                .Heating(createHouseCommand.Heating)
                .Elevator(createHouseCommand.Elevator)
                .Build();

            house.Dimension = CreateDimensionInstance(createHouseCommand.DimensionType,
                createHouseCommand.DimensionStringValue,
                createHouseCommand.DimensionIntValue, house);

            return house;
        }

        /// <summary>
        /// Create a new Hostel instance
        /// </summary>
        /// <param name="createHostelCommand"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="genderRestriction"></param>
        /// <returns></returns>
        private Hostel CreateHostelInstance(CreateHostelCommand createHostelCommand, decimal latitude, decimal longitude,
            GenderRestriction genderRestriction)
        {
            Hostel hostel = new Hostel.HostelBuilder()
                .Title(createHostelCommand.Title)
                .Description(createHostelCommand.Description)
                .CableTvAvailable(createHostelCommand.CableTvAvailable)
                .ParkingAvailable(createHostelCommand.ParkingAvailable)
                .WithInternetAvailable(createHostelCommand.InternetAvailable)
                .RentPrice(createHostelCommand.RentPrice)
                .OwnerEmail(createHostelCommand.OwnerEmail)
                .OwnerPhoneNumber(createHostelCommand.OwnerPhoneNumber)
                .OwnerName(createHostelCommand.OwnerName)
                .PropertyType(createHostelCommand.PropertyType)
                .Latitude(latitude)
                .Longitude(longitude)
                .Area(createHostelCommand.Area)
                .GenderRestriction(genderRestriction)
                .IsShared(createHostelCommand.IsShared)
                .RentUnit(createHostelCommand.RentUnit)
                .LandlineNumber(createHostelCommand.LandlineNumber)
                .Fax(createHostelCommand.Fax)
                .Laundry(createHostelCommand.Laundry)
                .AC(createHostelCommand.AC)
                .Geyser(createHostelCommand.Geyser)
                .AttachedBathroom(createHostelCommand.AttachedBathroom)
                .FitnessCentre(createHostelCommand.FitnessCentre)
                .Ironing(createHostelCommand.Ironing)
                .CctvCameras(createHostelCommand.CctvCameras)
                .BackupElectricity(createHostelCommand.BackupElectricity)
                .Balcony(createHostelCommand.Balcony)
                .Lawn(createHostelCommand.Lawn)
                .Heating(createHostelCommand.Heating)
                .Elevator(createHostelCommand.Elevator)
                .Meals(createHostelCommand.Meals)
                .NumberOfSeats(createHostelCommand.NumberOfSeats)
                .PicknDrop(createHostelCommand.PicknDrop)
                .Build();
            return hostel;
        }

        /// <summary>
        /// Create a new instance of Hotel
        /// </summary>
        /// <param name="createHotelCommand"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="genderRestriction"></param>
        /// <returns></returns>
        private Hotel CreateHotelInstance(CreateHotelCommand createHotelCommand, decimal latitude, decimal longitude,
            GenderRestriction genderRestriction)
        {
            Hotel hotel = new Hotel.HotelBuilder()
                .Title(createHotelCommand.Title)
                .Description(createHotelCommand.Description)
                .CableTvAvailable(createHotelCommand.CableTvAvailable)
                .ParkingAvailable(createHotelCommand.ParkingAvailable)
                .WithInternetAvailable(createHotelCommand.InternetAvailable)
                .RentPrice(createHotelCommand.RentPrice)
                .OwnerEmail(createHotelCommand.OwnerEmail)
                .OwnerPhoneNumber(createHotelCommand.OwnerPhoneNumber)
                .OwnerName(createHotelCommand.OwnerName)
                .PropertyType(createHotelCommand.PropertyType)
                .Latitude(latitude)
                .Longitude(longitude)
                .Area(createHotelCommand.Area)
                .GenderRestriction(genderRestriction)
                .IsShared(createHotelCommand.IsShared)
                .RentUnit(createHotelCommand.RentUnit)
                .LandlineNumber(createHotelCommand.LandlineNumber)
                .Fax(createHotelCommand.Fax)
                .Laundry(createHotelCommand.Laundry)
                .AC(createHotelCommand.AC)
                .Geyser(createHotelCommand.Geyser)
                .AttachedBathroom(createHotelCommand.AttachedBathroom)
                .FitnessCentre(createHotelCommand.FitnessCentre)
                .Ironing(createHotelCommand.Ironing)
                .CctvCameras(createHotelCommand.CctvCameras)
                .BackupElectricity(createHotelCommand.BackupElectricity)
                .Balcony(createHotelCommand.Balcony)
                .Lawn(createHotelCommand.Lawn)
                .Heating(createHotelCommand.Heating)
                .Elevator(createHotelCommand.Elevator)
                .Restaurant(createHotelCommand.Restaurant)
                .AirportShuttle(createHotelCommand.AirportShuttle)
                .BreakfastIncluded(createHotelCommand.BreakfastIncluded)
                .SittingArea(createHotelCommand.SittingArea)
                .CarRental(createHotelCommand.CarRental)
                .Spa(createHotelCommand.Spa)
                .Salon(createHotelCommand.Salon)
                .Bathtub(createHotelCommand.Bathtub)
                .SwimmingPool(createHotelCommand.SwimmingPool)
                .Kitchen(createHotelCommand.Kitchen)
                .NumberOfSingleBeds(createHotelCommand.NumberOfSingleBeds)
                .NumberOfDoubleBeds(createHotelCommand.NumberOfDoubleBeds)
                .Build();
            createHotelCommand.Occupants.Hotel = hotel;
            hotel.Occupants = createHotelCommand.Occupants;
            return hotel;
        }

        #endregion Create Property Helper Methods

        #region Update Property Helper Methods

        /// <summary>
        /// Update the House requested to the given attributes
        /// </summary>
        /// <param name="updateHouseCommand"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="genderRestriction"></param>
        private void UpdateHouse(UpdateHouseCommand updateHouseCommand, decimal latitude, decimal longitude,
            GenderRestriction genderRestriction)
        {
            House house = (House)_residentialPropertyRepository.GetPropertyById(updateHouseCommand.Id);
            if (house == null)
            {
                throw new InstanceNotFoundException($"House not found for HouseId: {updateHouseCommand.Id}");
            }
            else
            {
                // Check that this Update instance has the same email as the one associated with the requested 
                // house
                if (!updateHouseCommand.OwnerEmail.Equals(house.OwnerEmail))
                {
                    // These emails must match because this email is only related to account and not for display,
                    // once an account is created with an email it cannot be changed
                    throw new InvalidOperationException("Emails in original House and updated House do not match");
                }
            }

            Dimension dimension = CreateDimensionInstance(updateHouseCommand.DimensionType,
                updateHouseCommand.DimensionStringValue, updateHouseCommand.DimensionIntValue, house);
            house.UpdateHouse(updateHouseCommand.Title,
                updateHouseCommand.RentPrice,
                updateHouseCommand.NumberOfBedrooms,
                updateHouseCommand.NumberOfKitchens,
                updateHouseCommand.NumberOfBathrooms,
                updateHouseCommand.InternetAvailable,
                updateHouseCommand.LandlinePhoneAvailable,
                updateHouseCommand.CableTvAvailable,
                dimension,
                updateHouseCommand.GarageAvailable,
                updateHouseCommand.SmokingAllowed,
                updateHouseCommand.PropertyType,
                updateHouseCommand.OwnerEmail.ToLower(),
                updateHouseCommand.OwnerPhoneNumber,
                updateHouseCommand.HouseNo,
                updateHouseCommand.StreetNo,
                updateHouseCommand.Area,
                updateHouseCommand.OwnerName,
                updateHouseCommand.Description,
                genderRestriction,
                latitude,
                longitude,
                updateHouseCommand.IsShared,
                updateHouseCommand.RentUnit,
                updateHouseCommand.LandlineNumber,
                updateHouseCommand.Fax,
                updateHouseCommand.AC,
                updateHouseCommand.Geyser,
                updateHouseCommand.Balcony,
                updateHouseCommand.Lawn,
                updateHouseCommand.CctvCameras,
                updateHouseCommand.BackupElectricity,
                updateHouseCommand.Heating,
                updateHouseCommand.Bathtub,
                updateHouseCommand.Elevator);

            // Save the new house instance
            _residentialPropertyRepository.SaveorUpdate(house);
            _logger.Info("House updated Successfully: {0}", house);
        }

        private void UpdateHostel(UpdateHostelCommand updateHostelCommand, decimal latitude, decimal longitude,
            GenderRestriction genderRestriction)
        {
            Hostel hostel = (Hostel)_residentialPropertyRepository.GetPropertyById(updateHostelCommand.Id);
            if (hostel == null)
            {
                throw new InstanceNotFoundException($"Hostel not found for Id: {updateHostelCommand.Id}");
            }
            else
            {
                // Check that this Update instance has the same email as the one associated with the requested 
                // house
                if (!updateHostelCommand.OwnerEmail.Equals(hostel.OwnerEmail))
                {
                    // These emails must match because this email is only related to account and not for display,
                    // once an account is created with an email it cannot be changed
                    throw new InvalidOperationException("Emails in original Hostel and updated Hostel do not match");
                }
            }
            
            hostel.Update(updateHostelCommand.Title,
                            updateHostelCommand.RentPrice,
                            updateHostelCommand.OwnerEmail,
                updateHostelCommand.OwnerPhoneNumber,
                latitude,
                longitude,
                updateHostelCommand.Area,
                updateHostelCommand.OwnerName,
                updateHostelCommand.Description,
                genderRestriction,
                updateHostelCommand.IsShared,
                updateHostelCommand.RentUnit,
                updateHostelCommand.InternetAvailable,
                updateHostelCommand.CableTvAvailable,
                updateHostelCommand.ParkingAvailable,
                updateHostelCommand.PropertyType,
                updateHostelCommand.Laundry,
                updateHostelCommand.AC,
                updateHostelCommand.Geyser,
                updateHostelCommand.FitnessCentre,
                updateHostelCommand.AttachedBathroom,
                updateHostelCommand.Ironing,
                updateHostelCommand.Balcony,
                updateHostelCommand.Lawn,
                updateHostelCommand.CctvCameras,
                updateHostelCommand.BackupElectricity,
                updateHostelCommand.Meals,
                updateHostelCommand.PicknDrop,
                updateHostelCommand.NumberOfSeats,
                updateHostelCommand.Heating,
                updateHostelCommand.LandlineNumber,
                updateHostelCommand.Fax,
                updateHostelCommand.Elevator);

            _residentialPropertyRepository.SaveorUpdate(hostel);
            _logger.Info("Hostel updated Successfully: {0}", hostel);
        }

        /// <summary>
        /// Update Hotel
        /// </summary>
        /// <param name="updateHotelCommand"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="genderRestriction"></param>
        private void UpdateHotel(UpdateHotelCommand updateHotelCommand, decimal latitude, decimal longitude,
            GenderRestriction genderRestriction)
        {
            Hotel hotel = (Hotel)_residentialPropertyRepository.GetPropertyById(updateHotelCommand.Id);
            if (hotel == null)
            {
                throw new InstanceNotFoundException($"Hotel not found for Id: {updateHotelCommand.Id}");
            }
            else
            {
                // Check that this Update instance has the same email as the one associated with the requested 
                // house
                if (!updateHotelCommand.OwnerEmail.Equals(hotel.OwnerEmail))
                {
                    // These emails must match because this email is only related to account and not for display,
                    // once an account is created with an email it cannot be changed
                    throw new InvalidOperationException("Emails in original Hotel and updated Hotel do not match");
                }
            }
            
            hotel.Update(updateHotelCommand.Title,
                updateHotelCommand.RentPrice,
                updateHotelCommand.OwnerEmail,
                updateHotelCommand.OwnerPhoneNumber,
                latitude,
                longitude,
                updateHotelCommand.Area,
                updateHotelCommand.OwnerName,
                updateHotelCommand.Description,
                genderRestriction,
                updateHotelCommand.IsShared,
                updateHotelCommand.RentUnit,
                updateHotelCommand.InternetAvailable,
                updateHotelCommand.CableTvAvailable,
                updateHotelCommand.ParkingAvailable,
                updateHotelCommand.PropertyType,
                updateHotelCommand.Laundry,
                updateHotelCommand.AC,
                updateHotelCommand.Geyser,
                updateHotelCommand.FitnessCentre,
                updateHotelCommand.AttachedBathroom,
                updateHotelCommand.Ironing,
                updateHotelCommand.Balcony,
                updateHotelCommand.Lawn,
                updateHotelCommand.CctvCameras,
                updateHotelCommand.BackupElectricity,
                updateHotelCommand.Heating,
                updateHotelCommand.Restaurant,
                updateHotelCommand.AirportShuttle,
                updateHotelCommand.BreakfastIncluded,
                updateHotelCommand.SittingArea,
                updateHotelCommand.CarRental,
                updateHotelCommand.Spa,
                updateHotelCommand.Salon,
                updateHotelCommand.Bathtub,
                updateHotelCommand.SwimmingPool,
                updateHotelCommand.Kitchen,
                updateHotelCommand.NumberOfSingleBeds,
                updateHotelCommand.NumberOfDoubleBeds,
                updateHotelCommand.Occupants,
                updateHotelCommand.LandlineNumber,
                updateHotelCommand.Fax,
                updateHotelCommand.Elevator);
            
            _residentialPropertyRepository.SaveorUpdate(hotel);
            _logger.Info("Hotel updated Successfully: {0}", hotel);
        }
        
        #endregion Update Property Helper Methods

        #region Miscellaneous Helper Methods

        /// <summary>
        /// Create a new Instance for Dimension
        /// </summary>
        /// <param name="dimensionType"></param>
        /// <param name="dimensionStringValue"></param>
        /// <param name="dimensionDecimalValue"></param>
        /// <param name="house"></param>
        /// <returns></returns>
        private Dimension CreateDimensionInstance(string dimensionType, string dimensionStringValue,
            decimal dimensionDecimalValue,
            House house)
        {
            if (!string.IsNullOrWhiteSpace(dimensionType))
            {
                if (string.IsNullOrWhiteSpace(dimensionStringValue))
                {
                    throw new NullReferenceException(
                        $"DimensionStringValue cannot be null if the DimensionType is not null. OwnerEmail: {house.OwnerEmail}");
                }
                var dimension = new Dimension((DimensionType)Enum.Parse(typeof(DimensionType), dimensionType),
                    dimensionStringValue, dimensionDecimalValue, house);
                //_houseRepository.SaveorUpdateDimension(dimension);
                house.Dimension = dimension;
                return dimension;
            }
            return null;
        }

        /// <summary>
        /// Checks that the current user's email and the request house's uploader emails are the same
        /// </summary>
        /// <param name="houseOwnerEmail"></param>
        /// <returns></returns>
        private void CheckOwnerEmailIntegrity(string currentUserEmail, string houseOwnerEmail)
        {
            // Check if the current caller is using his own email in the CreateHouseCommand to upload a new house
            if (!currentUserEmail.Equals(houseOwnerEmail))
            {
                _logger.Error(
                    "Current user cannot upload house using another user's email. CurrentUser:{0} | HouseOwner:{1}",
                    currentUserEmail, houseOwnerEmail);
                throw new InvalidOperationException("Current user cannot upload house using another user's email.");
            }
        }

        #endregion Miscellaneous Helper Methods

        #region Convert To Representations

        /// <summary>
        /// Convert House to it's full representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private HouseFullRepresentation ConvertHouseToRepresentation(string id)
        {
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_residentialPropertyRepository.GetPropertyById(id);

            if (property == null)
            {
                return null;
            }
            House house = (House)property;
            string dimension = null;
            if (house.Dimension != null)
            {
                dimension = house.Dimension.StringValue + " " + house.Dimension.DimensionType;
            }

            return new HouseFullRepresentation(house.Id, house.Title, house.RentPrice, house.NumberOfBedrooms,
                house.NumberOfKitchens, house.NumberOfBathrooms, house.InternetAvailable,
                house.LandlinePhoneAvailable, house.CableTvAvailable, dimension, 
                house.GarageAvailable, house.SmokingAllowed, house.PropertyType, house.OwnerEmail, 
                house.OwnerPhoneNumber, house.HouseNo, house.StreetNo, house.Area,
                house.GetImageList(), house.OwnerName, house.Description, 
                house.GenderRestriction.ToString(), house.IsShared,
                house.RentUnit, house.LandlineNumber, house.Fax, house.AC, house.Geyser, house.Balcony,
                house.Lawn, house.CctvCameras, house.BackupElectricity, house.Heating, house.Bathtub,
                house.Elevator);
        }

        /// <summary>
        /// Convert the Hostel to it's full DTO Representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private HostelFullRepresentation ConvertHostelToRepresentation(string id)
        {
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_residentialPropertyRepository.GetPropertyById(id);

            if (property == null)
            {
                return null;
            }
            Hostel hostel = (Hostel) property;
            
            return new HostelFullRepresentation(hostel.Id, hostel.Title, hostel.RentPrice, hostel.OwnerEmail,
                hostel.OwnerPhoneNumber, hostel.Latitude, hostel.Longitude, hostel.Area, hostel.OwnerName, 
                hostel.Description, hostel.GenderRestriction.ToString(), hostel.IsShared, hostel.RentUnit, hostel.InternetAvailable,
                hostel.CableTvAvailable, hostel.ParkingAvailable, hostel.PropertyType, hostel.Laundry, hostel.AC,
                hostel.Geyser, hostel.FitnessCentre, hostel.AttachedBathroom, hostel.Ironing, hostel.Balcony, hostel.Lawn,
                hostel.CctvCameras, hostel.BackupElectricity, hostel.Heating, hostel.Meals, hostel.PicknDrop, 
                hostel.NumberOfSeats, hostel.LandlineNumber, hostel.Fax, hostel.Elevator, hostel.Images);
        }

        /// <summary>
        /// Convert the Hotel to it's full DTO Representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private HotelFullRepresentation ConvertHotelToRepresentation(string id)
        {
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_residentialPropertyRepository.GetPropertyById(id);

            if (property == null)
            {
                return null;
            }
            Hotel hotel = (Hotel)property;

            return new HotelFullRepresentation(hotel.Id, hotel.Title, hotel.RentPrice, hotel.OwnerEmail,
                hotel.OwnerPhoneNumber, hotel.Area, hotel.OwnerName, hotel.Description, hotel.GenderRestriction.ToString(),
                hotel.IsShared, hotel.RentUnit, hotel.InternetAvailable, hotel.CableTvAvailable, 
                hotel.ParkingAvailable, hotel.PropertyType, hotel.Laundry, hotel.AC, hotel.Geyser, 
                hotel.FitnessCentre, hotel.AttachedBathroom, hotel.Ironing, hotel.Balcony, hotel.Lawn,
                hotel.CctvCameras, hotel.BackupElectricity, hotel.Heating, hotel.Restaurant, hotel.AirportShuttle,
                hotel.BreakfastIncluded, hotel.SittingArea, hotel.CarRental, hotel.Spa, hotel.Salon, hotel.Bathtub,
                hotel.SwimmingPool, hotel.Kitchen, hotel.Occupants, hotel.LandlineNumber, hotel.Fax, hotel.Elevator, hotel.Images,
                hotel.NumberOfSingleBeds, hotel.NumberOfDoubleBeds);
        }

        private IList<ResidentialPropertyPartialBaseImplementation> ConvertPropertiesToPartialRepresentation(
            IList<Property.Domain.Model.PropertyAggregate.Property> properties)
        {
            IList<ResidentialPropertyPartialBaseImplementation> propertyRepresentations =
                new List<ResidentialPropertyPartialBaseImplementation>();
            foreach (var property in properties)
            {
                if (property is House)
                {
                    var houseRepresentation = ConvertHouseToPartialRepresentation((House) property);
                    propertyRepresentations.Add(houseRepresentation);
                }
                else if (property is Hostel)
                {
                    var hostelRepresentation = ConvertHostelToPartialRepresentation((Hostel) property);
                    propertyRepresentations.Add(hostelRepresentation);
                }
                else if (property is Hotel)
                {
                    var hotelRepresentations = ConvertHotelToPartialRepresentation((Hotel) property);
                    propertyRepresentations.Add(hotelRepresentations);
                }
            }
            return propertyRepresentations;
        }

        /// <summary>
        /// Converts the list of houses to a list of PartialHouseRepresentations
        /// </summary>
        /// <param name="houses"></param>
        /// <returns></returns>
        private IList<ResidentialPropertyPartialBaseImplementation> ConvertHousesToPartialRepresentations(IList<House> houses)
        {
            IList<ResidentialPropertyPartialBaseImplementation> houseRepresentations = new List<ResidentialPropertyPartialBaseImplementation>();
            if (houses != null && houses.Count > 0)
            {
                foreach (var house in houses)
                {
                    var houseRepresentation = ConvertHouseToPartialRepresentation(house);

                    houseRepresentations.Add(houseRepresentation);
                }
            }
            return houseRepresentations;
        }

        /// <summary>
        /// Converts a sinlge house to a representation
        /// </summary>
        /// <returns></returns>
        private HousePartialRepresentation ConvertHouseToPartialRepresentation(House house)
        {
            string firstImage = null;
            if (house.GetImageList() != null && house.GetImageList().Count > 0)
            {
                firstImage = house.GetImageList()[0];
            }
            HousePartialRepresentation houseRepresentation = new HousePartialRepresentation(
                house.Id, house.Title, house.Area, house.RentPrice, house.PropertyType, house.Dimension,
                house.OwnerPhoneNumber, house.LandlineNumber, firstImage, house.OwnerName,
                house.IsShared, house.GenderRestriction.ToString(), house.RentUnit, house.InternetAvailable,
                house.CableTvAvailable, house.NumberOfBedrooms, house.NumberOfBathrooms,
                house.NumberOfKitchens, house.AC, house.Geyser, house.Balcony, house.Lawn, house.CctvCameras,
                house.BackupElectricity, house.Heating, house.Bathtub, house.Elevator, house.GarageAvailable,
                house.LandlinePhoneAvailable);
            return houseRepresentation;
        }

        /// <summary>
        /// Convert Hostels to their partial representations
        /// </summary>
        /// <param name="hostels"></param>
        /// <returns></returns>
        private IList<ResidentialPropertyPartialBaseImplementation> ConvertHostelsToPartialRepresentations(IList<Hostel> hostels)
        {
            IList<ResidentialPropertyPartialBaseImplementation> hostelRepresentations = new List<ResidentialPropertyPartialBaseImplementation>();
            if (hostels != null && hostels.Count > 0)
            {
                foreach (var hostel in hostels)
                {
                    var hostelRepresentation = ConvertHostelToPartialRepresentation(hostel);

                    hostelRepresentations.Add(hostelRepresentation);
                }
            }
            return hostelRepresentations;
        }

        /// <summary>
        /// Converts a single hostel to a representation
        /// </summary>
        /// <returns></returns>
        private HostelPartialRepresentation ConvertHostelToPartialRepresentation(Hostel hostel)
        {
            string firstImage = null;
            if (hostel.GetImageList() != null && hostel.GetImageList().Count > 0)
            {
                firstImage = hostel.GetImageList()[0];
            }
            HostelPartialRepresentation hostelRepresentation = new HostelPartialRepresentation(
                hostel.Id, hostel.Title, hostel.RentPrice, hostel.OwnerPhoneNumber,
                hostel.LandlineNumber, hostel.Area, hostel.OwnerName, hostel.GenderRestriction.ToString(),
                hostel.IsShared, hostel.RentUnit, hostel.InternetAvailable, hostel.CableTvAvailable,
                hostel.PropertyType, hostel.ParkingAvailable, hostel.Laundry, hostel.AC, hostel.Geyser,
                hostel.AttachedBathroom, hostel.BackupElectricity, hostel.Meals, hostel.NumberOfSeats,
                firstImage);
            return hostelRepresentation;
        }

        /// <summary>
        /// Converts Hotel & Gueast House Domain Objects to their representation
        /// </summary>
        /// <param name="hotels"></param>
        /// <returns></returns>
        private IList<ResidentialPropertyPartialBaseImplementation> ConvertHotelsToPartialRepresentations(IList<Hotel> hotels)
        {
            IList<ResidentialPropertyPartialBaseImplementation> hostelRepresentations = new List<ResidentialPropertyPartialBaseImplementation>();
            if (hotels != null && hotels.Count > 0)
            {
                foreach (var hotel in hotels)
                {
                    var hotelRepresentations = ConvertHotelToPartialRepresentation(hotel);

                    hostelRepresentations.Add(hotelRepresentations);
                }
            }
            return hostelRepresentations;
        }

        /// <summary>
        /// Converts a single hotel to a representation
        /// </summary>
        /// <returns></returns>
        private HotelPartialRepresentation ConvertHotelToPartialRepresentation(Hotel hotel)
        {
            string firstImage = null;
            if (hotel.GetImageList() != null && hotel.GetImageList().Count > 0)
            {
                firstImage = hotel.GetImageList()[0];
            }
            HotelPartialRepresentation hotelRepresentation = new HotelPartialRepresentation(
                hotel.Id, hotel.Title, hotel.RentPrice, hotel.OwnerPhoneNumber,
                hotel.Area, hotel.OwnerName, hotel.GenderRestriction.ToString(),
                hotel.IsShared, hotel.RentUnit, hotel.InternetAvailable, hotel.CableTvAvailable,
                hotel.ParkingAvailable, hotel.PropertyType, hotel.AC, hotel.Geyser,
                hotel.AttachedBathroom, hotel.FitnessCentre, hotel.BackupElectricity, hotel.Heating, hotel.AirportShuttle,
                hotel.BreakfastIncluded, hotel.Occupants, hotel.LandlineNumber, firstImage, 
                hotel.NumberOfSingleBeds, hotel.NumberOfDoubleBeds);
            return hotelRepresentation;
        }
        
        #endregion Convert to representations
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;
using Newtonsoft.Json;
using NLog;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.PropertyServices.Commands.CreateCommands;
using RentStuff.Property.Application.PropertyServices.Commands.UpdateCommands;
using RentStuff.Property.Application.PropertyServices.Representation;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;
using RentStuff.Property.Application.PropertyServices.Representation.FullRepresentations;
using RentStuff.Property.Application.PropertyServices.Representation.PartialRepresentations;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HotelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace RentStuff.Property.Application.PropertyServices
{
    /// <summary>
    /// House Application Service
    /// </summary>
    public class PropertyApplicationService : IPropertyApplicationService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IResidentialPropertyRepository _houseRepository;
        private RentStuff.Common.Services.LocationServices.IGeocodingService _geocodingService;
        private RentStuff.Common.Services.GoogleStorageServices.IPhotoStorageService _photoStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PropertyApplicationService(IResidentialPropertyRepository houseRepository,
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService,
            RentStuff.Common.Services.GoogleStorageServices.IPhotoStorageService photoStorageService)
        {
            _houseRepository = houseRepository;
            _geocodingService = geocodingService;
            _photoStorageService = photoStorageService;
        }

        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        public string SaveNewProperty(dynamic propertyJson, string currentUserEmail)
        {
            dynamic propertyBaseCommand = JsonConvert.DeserializeObject<object>(propertyJson);
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

            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal, decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(propertyBaseCommand.Area.Value);
            GenderRestriction genderRestriction = default(GenderRestriction);

            if (!string.IsNullOrWhiteSpace(propertyBaseCommand.GenderRestriction.Value))
            {
                Enum.TryParse(propertyBaseCommand.GenderRestriction.Value, out genderRestriction);
            }
            // The Id of whichever property gets created
            string id = null;
            // Now check what type of command it is, and cast it to that property type
            if (propertyBaseCommand.PropertyType.Value.Equals(Constants.House) ||
                propertyBaseCommand.PropertyType.Value.Equals(Constants.Apartment))
            {
                CreateHouseCommand createHouseCommand = (CreateHouseCommand)propertyBaseCommand;
                House house = CreateHouseInstance(createHouseCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
                // Save the new house instance
                _houseRepository.SaveorUpdate(house);
                _logger.Info("House uploaded Successfully: {0}", house);
                id = house.Id;
            }
            // If the request is for creating a new Hostel
            else if (propertyBaseCommand.PropertyType.Value.Equals(Constants.Hostel))
            {
                CreateHostelCommand createHostelCommand = (CreateHostelCommand) propertyBaseCommand;
                Hostel hostel = CreateHostelInstance(createHostelCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
                _houseRepository.SaveorUpdate(hostel);
                _logger.Info("Hostel uploaded Successfully: {0}", hostel);

                id = hostel.Id;
            }
            // If the request is for creating a new Hotel Guest House
            else if (propertyBaseCommand.PropertyType.Value.Equals(Constants.Hotel) ||
                     propertyBaseCommand.PropertyType.Value.Equals(Constants.GuestHouse))
            {
                CreateHotelCommand createHotelCommand = (CreateHotelCommand)propertyBaseCommand;
                Hotel hotel = CreateHotelInstance(createHotelCommand, coordinates.Item1, coordinates.Item2,
                    genderRestriction);
                _houseRepository.SaveorUpdate(hotel);
                _logger.Info("Hotel uploaded Successfully: {0}", hotel);

                id = hotel.Id;
            }
            else
            {
                throw new NotImplementedException("Only residential Property types are supported yet");
            }

            return id;
        }

        /// <summary>
        /// Updates an exisitng house
        /// </summary>
        /// <param name="updateHouseCommand"></param>
        /// <returns></returns>
        public bool UpdateHouse(UpdateHouseCommand updateHouseCommand)
        {
            House house = (House)_houseRepository.GetPropertyById(updateHouseCommand.Id);
            if (house == null)
            {
                throw new InstanceNotFoundException($"House not found for HouseId: {updateHouseCommand.Id}");
            }
            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal, decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(updateHouseCommand.Area);

            if (coordinates == null || coordinates.Item1 == decimal.Zero || coordinates.Item2 == decimal.Zero)
            {
                throw new InvalidDataException(
                    $"Could not find coordinates from the given address: {updateHouseCommand.Area}");
            }
            
            GenderRestriction genderRestriction =
                (GenderRestriction) Enum.Parse(typeof(GenderRestriction), updateHouseCommand.GenderRestriction);

            Dimension dimension = CreateDimensionInstance(updateHouseCommand.DimensionType,
                updateHouseCommand.DimensionStringValue, updateHouseCommand.DimensionIntValue, house);
            house.UpdateHouse(updateHouseCommand.Title, 
                updateHouseCommand.RentPrice,
                updateHouseCommand.NumberOfBedrooms,
                updateHouseCommand.NumberOfKitchens, 
                updateHouseCommand.NumberOfBathrooms,
                updateHouseCommand.InternetAvailable,
                updateHouseCommand.LandlinePhoneAvailable, 
                updateHouseCommand.CableTvAvailable, dimension,
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
                genderRestriction, coordinates.Item1, 
                coordinates.Item2, 
                updateHouseCommand.IsShared,
                updateHouseCommand.RentUnit, 
                updateHouseCommand.LandlineNumber,
                updateHouseCommand.Fax);

            _houseRepository.SaveorUpdate(house);
            _logger.Info("Updated House successfully: {0}", house);
            return true;
        }

        /// <summary>
        /// Delete the given house instance
        /// </summary>
        /// <param name="houseId"></param>
        public void DeleteHouse(string houseId)
        {
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_houseRepository.GetPropertyById(houseId);
            if (property != null)
            {
                // Delete all the images from the Google cloud storage photo bucket
                foreach (var image in property.Images)
                {
                    _photoStorageService.DeletePhoto(image);
                }
                _houseRepository.Delete(property);
                _logger.Info("Deleted property successfully: {0}", property);
            }
            else
            {
                throw new InstanceNotFoundException($"No property could be found for the given id. HouseId: {houseId}");
            }
        }

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        public IList<ResidentialPropertyPartialBaseImplementation> GetPropertiesByEmail(string propertyType, string email,
            int pageNo = 0)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new NullReferenceException("Email cannot be null");
            }
            switch (propertyType)
            {
                case Constants.House:
                    var houses = _houseRepository.GetHouseByOwnerEmail(email, pageNo);
                    return ConvertHousesToPartialRepresentations(houses);
                case Constants.Apartment:
                    var apartments = _houseRepository.GetHouseByOwnerEmail(email, pageNo);
                    return ConvertHousesToPartialRepresentations(apartments);
                case Constants.Hostel:
                    var hostels = _houseRepository.GetHostelsByOwnerEmail(email, pageNo);
                    return ConvertHostelsToPartialRepresentations(hostels);
                case Constants.Hotel:
                    var hotels = _houseRepository.GetHotelsByOwnerEmail(email, pageNo);
                    return ConvertHotelsToPartialRepresentations(hotels);
                case Constants.GuestHouse:
                    var guestHouses = _houseRepository.GetHotelsByOwnerEmail(email, pageNo);
                    return ConvertHotelsToPartialRepresentations(guestHouses);
                default:
                    throw new NotImplementedException("Requested Proeprty type is not supported");
            }
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
        public IList<ResidentialPropertyPartialBaseImplementation> SearchPropertiesByPropertyType(string propertyType, int pageNo = 0)
        {
            switch (propertyType)
            {
                case Constants.House:
                    var houses = _houseRepository.GetAllHouses(pageNo);
                    return ConvertHousesToPartialRepresentations(houses);
                case Constants.Apartment:
                    var apartments = _houseRepository.GetAllHouses(pageNo);
                    return ConvertHousesToPartialRepresentations(apartments);
                case Constants.Hostel:
                    var hostels = _houseRepository.GetAllHostels(pageNo);
                    return ConvertHostelsToPartialRepresentations(hostels);
                case Constants.Hotel:
                    var hotels = _houseRepository.GetAllHotels(pageNo);
                    return ConvertHotelsToPartialRepresentations(hotels);
                case Constants.GuestHouse:
                    var guestHouses = _houseRepository.GetAllHotels(pageNo);
                    return ConvertHotelsToPartialRepresentations(guestHouses);
                default:
                    throw new NotImplementedException("Requested Proeprty type is not supported");
            }
        }

        /// <summary>
        /// Search nearby houses by providing the area and property type
        /// </summary>
        /// <param name="address"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<ResidentialPropertyPartialBaseImplementation> SearchPropertiesByAreaAndPropertyType(string address, 
            string propertyType, int pageNo = 0)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);

            switch (propertyType)
            {
                case Constants.House:
                    var houses = _houseRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    return ConvertHousesToPartialRepresentations(houses);
                case Constants.Apartment:
                    var apartments = _houseRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    return ConvertHousesToPartialRepresentations(apartments);
                case Constants.Hostel:
                    var hostels = _houseRepository.SearchHostelByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    return ConvertHostelsToPartialRepresentations(hostels);
                case Constants.Hotel:
                    var hotels = _houseRepository.SearchHotelByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    return ConvertHotelsToPartialRepresentations(hotels);
                case Constants.GuestHouse:
                    var guestHouses = _houseRepository.SearchHotelByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    return ConvertHotelsToPartialRepresentations(guestHouses);
            }
            throw new NotImplementedException("Requested property Type is not supported");
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
                // And property type is also not null
                if (!string.IsNullOrWhiteSpace(propertyType))
                {
                    recordCount = _houseRepository.GetRecordCountByLocationAndPropertyType(coordinates.Item1,
                        coordinates.Item2,
                        propertyType);
                    return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
                }
                // otherwise just get the count for houses given the coordinates
                else
                {
                    recordCount = _houseRepository.GetRecordCountByLocation(coordinates.Item1, coordinates.Item2);
                    return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
                }
            }
            if (!string.IsNullOrWhiteSpace(propertyType))
            {
                recordCount = _houseRepository.GetRecordCountByPropertyType(propertyType);
                return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                recordCount = _houseRepository.GetRecordCountByEmail(email);
                return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
            }
            // If no criteria is given, return the total number of houses present in the database
            recordCount = _houseRepository.GetTotalRecordCount();
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
            House house = (House)_houseRepository.GetPropertyById(houseId);
            // If we find a hosue with the given ID
            if (house != null)
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
                house.AddImage(fileName);
                // Save the updated house in the repository
                _houseRepository.SaveorUpdate(house);
                // Log the info
                _logger.Info("Added images to house successfully. HouseId: {0}", house.Id);
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
            var house = (ResidentialProperty)_houseRepository.GetPropertyById(houseId);
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
                _houseRepository.SaveorUpdate(house);
                _logger.Info("Deleted images from house successfully. HouseId: {0}", house.Id);
            }
        }

        /// <summary>
        /// Check that the given email is the same as the owner of the given house id
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="requesterEmail"></param>
        /// <returns></returns>
        public bool HouseOwnershipCheck(string houseId, string requesterEmail)
        {
            var house = (ResidentialProperty)_houseRepository.GetPropertyById(houseId);
            if (house == null)
            {
                throw new NullReferenceException($"No house found for the given HouseId: {houseId}");
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
                    string firstImage = null;
                    if (house.GetImageList() != null && house.GetImageList().Count > 0)
                    {
                       firstImage =  house.GetImageList()[0];
                    }
                    HousePartialRepresentation houseRepresentation = new HousePartialRepresentation(
                        house.Id, house.Title, house.Area, house.RentPrice, house.PropertyType, house.Dimension,
                        house.OwnerPhoneNumber, house.LandlineNumber, firstImage, house.OwnerName,
                        house.IsShared, house.GenderRestriction.ToString(), house.RentUnit, house.InternetAvailable, 
                        house.CableTvAvailable, house.NumberOfBedrooms, house.NumberOfBathrooms,
                        house.NumberOfKitchens);
                    
                    houseRepresentations.Add(houseRepresentation);
                }
            }
            return houseRepresentations;
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
                    string firstImage = null;
                    if (hostel.GetImageList() != null && hostel.GetImageList().Count > 0)
                    {
                        firstImage = hostel.GetImageList()[0];
                    }
                    HostelPartialRepresentation houseRepresentation = new HostelPartialRepresentation(
                        hostel.Id, hostel.Title, hostel.RentPrice, hostel.OwnerPhoneNumber,
                        hostel.LandlineNumber, hostel.Area, hostel.OwnerName, hostel.GenderRestriction.ToString(), 
                        hostel.IsShared, hostel.RentUnit, hostel.InternetAvailable, hostel.CableTvAvailable,
                        hostel.PropertyType, hostel.ParkingAvailable, hostel.Laundry, hostel.AC, hostel.Geyser, 
                        hostel.AttachedBathroom, hostel.BackupElectricity, hostel.Meals, hostel.NumberOfSeats,
                        firstImage);

                    hostelRepresentations.Add(houseRepresentation);
                }
            }
            return hostelRepresentations;
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
                    string firstImage = null;
                    if (hotel.GetImageList() != null && hotel.GetImageList().Count > 0)
                    {
                        firstImage = hotel.GetImageList()[0];
                    }
                    HotelPartialRepresentation houseRepresentation = new HotelPartialRepresentation(
                        hotel.Id, hotel.Title, hotel.RentPrice, hotel.OwnerPhoneNumber,
                        hotel.Area, hotel.OwnerName, hotel.GenderRestriction.ToString(),
                        hotel.IsShared, hotel.RentUnit, hotel.InternetAvailable, hotel.CableTvAvailable,
                        hotel.ParkingAvailable, hotel.PropertyType, hotel.AC, hotel.Geyser,
                        hotel.AttachedBathroom, hotel.BackupElectricity, hotel.Heating, hotel.AirportShuttle,
                        hotel.BreakfastIncluded, hotel.Occupants, hotel.LandlineNumber, firstImage);

                    hostelRepresentations.Add(houseRepresentation);
                }
            }
            return hostelRepresentations;
        }

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
        /// <param name="createHostelCommand"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="genderRestriction"></param>
        /// <returns></returns>
        private Hotel CreateHotelInstance(CreateHotelCommand createHostelCommand, decimal latitude, decimal longitude,
            GenderRestriction genderRestriction)
        {
            Hotel hotel = new Hotel.HotelBuilder()
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
                .Restaurant(createHostelCommand.Restaurant)
                .AirportShuttle(createHostelCommand.AirportShuttle)
                .BreakfastIncluded(createHostelCommand.BreakfastIncluded)
                .SittingArea(createHostelCommand.SittingArea)
                .CarRental(createHostelCommand.CarRental)
                .Spa(createHostelCommand.Spa)
                .Salon(createHostelCommand.Salon)
                .Bathtub(createHostelCommand.Bathtub)
                .SwimmingPool(createHostelCommand.SwimmingPool)
                .Kitchen(createHostelCommand.Kitchen)
                .Occupants(new Occupants(createHostelCommand.NumberOfAdults, createHostelCommand.NumberOfChildren))
                .Build();
            return hotel;
        }

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

        #region Convert To Representations

        /// <summary>
        /// Convert House to it's full representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private HouseFullRepresentation ConvertHouseToRepresentation(string id)
        {
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_houseRepository.GetPropertyById(id);

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
                house.LandlinePhoneAvailable,
                house.CableTvAvailable, dimension, house.GarageAvailable, house.SmokingAllowed,
                house.PropertyType.ToString(),
                house.OwnerEmail, house.OwnerPhoneNumber, house.HouseNo,
                house.StreetNo,
                house.Area,
                house.GetImageList(), house.OwnerName, house.Description, house.GenderRestriction.ToString(),
                house.IsShared,
                house.RentUnit, house.LandlineNumber, house.Fax);
        }

        /// <summary>
        /// Convert the Hostel to it's full DTO Representation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private HostelFullRepresentation ConvertHostelToRepresentation(string id)
        {
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_houseRepository.GetPropertyById(id);

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
            Domain.Model.PropertyAggregate.Property property = (Domain.Model.PropertyAggregate.Property)_houseRepository.GetPropertyById(id);

            if (property == null)
            {
                return null;
            }
            Hotel house = (Hotel)property;

            return new HotelFullRepresentation(house.Id, house.Title, house.RentPrice, house.OwnerEmail,
                house.OwnerPhoneNumber, house.Area, house.OwnerName, house.Description, house.GenderRestriction.ToString(),
                house.IsShared, house.RentUnit, house.InternetAvailable, house.CableTvAvailable, 
                house.ParkingAvailable, house.PropertyType, house.Laundry, house.AC, house.Geyser, 
                house.FitnessCentre, house.AttachedBathroom, house.Ironing, house.Balcony, house.Lawn,
                house.CctvCameras, house.BackupElectricity, house.Heating, house.Restaurant, house.AirportShuttle,
                house.BreakfastIncluded, house.SittingArea, house.CarRental, house.Spa, house.Salon, house.Bathtub,
                house.SwimmingPool, house.Kitchen, house.Occupants, house.LandlineNumber, house.Fax, house.Elevator, house.Images);
        }

        #endregion Convert to representations
    }
}

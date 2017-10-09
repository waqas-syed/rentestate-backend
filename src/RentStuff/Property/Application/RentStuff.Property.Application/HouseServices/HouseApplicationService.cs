using NLog;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Representation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace RentStuff.Property.Application.HouseServices
{
    /// <summary>
    /// House Application Service
    /// </summary>
    public class HouseApplicationService : IHouseApplicationService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IResidentialPropertyRepository _houseRepository;
        private RentStuff.Common.Services.LocationServices.IGeocodingService _geocodingService;
        private RentStuff.Common.Services.GoogleStorageServices.IPhotoStorageService _photoStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HouseApplicationService(IResidentialPropertyRepository houseRepository,
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
        public string SaveNewHouseOffer(CreateHouseCommand createHouseCommand)
        {
            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal, decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(createHouseCommand.Area);

            if (coordinates == null || coordinates.Item1 == decimal.Zero || coordinates.Item2 == decimal.Zero)
            {
                _logger.Error("Error while getting coordinates for the given address. Address: {0} | OwnerEmail: {1}",
                    createHouseCommand.Area, createHouseCommand.OwnerEmail);
                throw new InvalidDataException($"Could not find coordinates from the given address." +
                                               $" Address: {createHouseCommand.Area} | OwnerEmail: {createHouseCommand.OwnerEmail}");
            }
            GenderRestriction genderRestriction = default(GenderRestriction);
            if (!string.IsNullOrWhiteSpace(createHouseCommand.GenderRestriction))
            {
                Enum.TryParse(createHouseCommand.GenderRestriction, out genderRestriction);
            }

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
                .Latitude(coordinates.Item1)
                .Longitude(coordinates.Item2)
                .HouseNo(createHouseCommand.HouseNo)
                .StreetNo(createHouseCommand.StreetNo)
                .Area(createHouseCommand.Area)
                .OwnerName(createHouseCommand.OwnerName)
                .GenderRestriction(genderRestriction)
                .IsShared(createHouseCommand.IsShared)
                .RentUnit(createHouseCommand.RentUnit)
                .Build();

            house.Dimension = CreateDimensionInstance(createHouseCommand.DimensionType,
                createHouseCommand.DimensionStringValue,
                createHouseCommand.DimensionIntValue, house);
            // Save the new house instance
            _houseRepository.SaveorUpdate(house);
            _logger.Info("House uploaded Successfully: {0}", house);

            return house.Id;
        }

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
                var dimension = new Dimension((DimensionType) Enum.Parse(typeof(DimensionType), dimensionType),
                    dimensionStringValue, dimensionDecimalValue, house);
                //_houseRepository.SaveorUpdateDimension(dimension);
                house.Dimension = dimension;
                return dimension;
            }
            return null;
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
            House house = (House)_houseRepository.GetPropertyById(houseId);
            if (house != null)
            {
                // Delete all the images from the Google cloud storage photo bucket
                foreach (var image in house.Images)
                {
                    _photoStorageService.DeletePhoto(image);
                }
                _houseRepository.Delete(house);
                _logger.Info("Deleted House successfully: {0}", house);
            }
            else
            {
                throw new InstanceNotFoundException($"No house could be found for the given id. HouseId: {houseId}");
            }
        }

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        public IList<HousePartialRepresentation> GetHouseByEmail(string email, int pageNo = 0)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new NullReferenceException("Email cannot be null");
            }
            IList<House> houses = _houseRepository.GetHouseByOwnerEmail(email.ToLower(), pageNo);
            return ConvertHousesToPartialRepresentations(houses);
        }

        /// <summary>
        /// Get House by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HouseFullRepresentation GetHouseById(string id)
        {
            House house = (House)_houseRepository.GetPropertyById(id);
            string dimension = null;
            if (house.Dimension != null)
            {
                dimension = house.Dimension.StringValue + " " + house.Dimension.DimensionType;
            }

            return new HouseFullRepresentation(house.Id, house.Title, house.RentPrice, house.NumberOfBedrooms,
                house.NumberOfKitchens, house.NumberOfBathrooms, house.InternetAvailable, house.LandlinePhoneAvailable,
                house.CableTvAvailable, dimension, house.GarageAvailable, house.SmokingAllowed,
                house.PropertyType.ToString(),
                house.OwnerEmail, house.OwnerPhoneNumber, house.Latitude, house.Longitude, house.HouseNo, house.StreetNo,
                house.Area,
                house.GetImageList(), house.OwnerName, house.Description, house.GenderRestriction.ToString(), house.IsShared,
                house.RentUnit, house.LandlineNumber, house.Fax);
        }
        
        /// <summary>
        /// Search houses with reference to propertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByPropertyType(string propertyType, int pageNo = 0)
        {
            if (propertyType.Equals(Constants.House) && propertyType.Equals(Constants.Apartment))
            {
                IList<House> houses = _houseRepository.GetAllHouses(pageNo);
                return ConvertHousesToPartialRepresentations(houses);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search nearby houses by providing the area and property type
        /// </summary>
        /// <param name="address"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByAreaAndPropertyType(string address, 
            string propertyType, int pageNo = 0)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);

            switch (propertyType)
            {
                case Constants.House:
                    _houseRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    break;
                case Constants.Apartment:
                    _houseRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    break;
                case Constants.Hostel:
                    _houseRepository.SearchHostelByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    break;
                case Constants.Hotel:
                    _houseRepository.SearchHotelByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    break;
                case Constants.GuestHouse:
                    _houseRepository.SearchHotelByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
                    break;
            }
            // Get 20 coordinates within the range of around 30 kilometers radius
            IList<House> houses = _houseRepository.SearchHousesByCoordinates(coordinates.Item1,
                coordinates.Item2, pageNo);
            return ConvertHousesToPartialRepresentations(houses);
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
        private IList<HousePartialRepresentation> ConvertHousesToPartialRepresentations(IList<House> houses)
        {
            IList<HousePartialRepresentation> houseRepresentations = new List<HousePartialRepresentation>();
            if (houses != null && houses.Count > 0)
            {
                foreach (var house in houses)
                {
                    string firstImage = null;
                    if (house.GetImageList() != null && house.GetImageList().Count > 0)
                    {
                       firstImage =  house.GetImageList()[0];
                    }
                    HousePartialRepresentation houseRepresentation = new HousePartialRepresentation(house.Id, house.Title, house.Area, 
                        house.RentPrice, house.PropertyType.ToString(), house.Dimension, house.NumberOfBedrooms, 
                        house.NumberOfBathrooms, house.NumberOfKitchens, house.OwnerEmail, house.OwnerPhoneNumber,
                        firstImage, house.OwnerName, house.Description, house.IsShared, house.GenderRestriction.ToString(),
                        house.RentUnit);
                    
                    houseRepresentations.Add(houseRepresentation);
                }
            }
            return houseRepresentations;
        }
    }
}

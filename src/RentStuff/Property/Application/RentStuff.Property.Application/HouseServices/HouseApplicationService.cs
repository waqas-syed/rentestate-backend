using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management.Instrumentation;
using System.Web.Hosting;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.Services;
using System.Linq;
using NLog;
using RentStuff.Common;
using RentStuff.Property.Application.HouseServices.Representation;

namespace RentStuff.Property.Application.HouseServices
{
    /// <summary>
    /// House Application Service
    /// </summary>
    public class HouseApplicationService : IHouseApplicationService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IHouseRepository _houseRepository;
        private IGeocodingService _geocodingService;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HouseApplicationService(IHouseRepository houseRepository, IGeocodingService geocodingService)
        {
            _houseRepository = houseRepository;
            _geocodingService = geocodingService;
        }

        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        public string SaveNewHouseOffer(CreateHouseCommand createHouseCommand)
        {
            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal,decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(createHouseCommand.Area);

            if (coordinates == null || coordinates.Item1 == decimal.Zero || coordinates.Item2 == decimal.Zero)
            {
                _logger.Error("Error while getting coordinates for the given address. Address: {0} | OwnerEmail: {1}",
                                                                        createHouseCommand.Area, createHouseCommand.OwnerEmail);
                throw new InvalidDataException($"Could not find coordinates from the given address." +
                                               $" Address: {createHouseCommand.Area} | OwnerEmail: {createHouseCommand.OwnerEmail}");
            }
            PropertyType propertyType = default(PropertyType);
            if (!string.IsNullOrEmpty(createHouseCommand.PropertyType))
            {
                Enum.TryParse(createHouseCommand.PropertyType, out propertyType);
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
                .MonthlyRent(createHouseCommand.MonthlyRent)
                .NumberOfBathrooms(createHouseCommand.NumberOfBathrooms)
                .NumberOfBedrooms(createHouseCommand.NumberOfBedrooms)
                .NumberOfKitchens(createHouseCommand.NumberOfKitchens)
                .OwnerEmail(createHouseCommand.OwnerEmail)
                .OwnerPhoneNumber(createHouseCommand.OwnerPhoneNumber)
                .SmokingAllowed(createHouseCommand.SmokingAllowed)
                .WithInternetAvailable(createHouseCommand.InternetAvailable)
                .PropertyType(propertyType)
                .Latitude(coordinates.Item1)
                .Longitude(coordinates.Item2)
                .HouseNo(createHouseCommand.HouseNo)
                .StreetNo(createHouseCommand.StreetNo)
                .Area(createHouseCommand.Area)
                .OwnerName(createHouseCommand.OwnerName)
                .GenderRestriction(genderRestriction).Build();

            house.Dimension = CreateDimensionInstance(createHouseCommand.DimensionType, createHouseCommand.DimensionStringValue,
                createHouseCommand.DimensionIntValue, house);
            // Save the new house instance
            _houseRepository.SaveorUpdate(house);
            _logger.Info("House uploaded Successfully: {0}", house);

            return house.Id;
        }

        private Dimension CreateDimensionInstance(string dimensionType, string dimensionStringValue, decimal dimensionDecimalValue,
            House house)
        {
            if (!string.IsNullOrWhiteSpace(dimensionType))
            {
                if (string.IsNullOrWhiteSpace(dimensionStringValue))
                {
                    throw new NullReferenceException($"DimensionStringValue cannot be null if the DimensionType is not null. OwnerEmail: {house.OwnerEmail}");
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
        /// Updates an exisitng house
        /// </summary>
        /// <param name="updateHouseCommand"></param>
        /// <returns></returns>
        public bool UpdateHouse(UpdateHouseCommand updateHouseCommand)
        {
            House house = _houseRepository.GetHouseById(updateHouseCommand.Id);
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
            PropertyType propertyType = (PropertyType)Enum.Parse(typeof(PropertyType), updateHouseCommand.PropertyType);
            GenderRestriction genderRestriction =
                (GenderRestriction) Enum.Parse(typeof(GenderRestriction), updateHouseCommand.GenderRestriction);

            Dimension dimension = CreateDimensionInstance(updateHouseCommand.DimensionType,
                updateHouseCommand.DimensionStringValue, updateHouseCommand.DimensionIntValue, house);
            house.UpdateHouse(updateHouseCommand.Title, updateHouseCommand.MonthlyRent, updateHouseCommand.NumberOfBedrooms,
                updateHouseCommand.NumberOfKitchens, updateHouseCommand.NumberOfBathrooms, updateHouseCommand.InternetAvailable,
                updateHouseCommand.LandlinePhoneAvailable, updateHouseCommand.CableTvAvailable, dimension, updateHouseCommand.GarageAvailable,
                updateHouseCommand.SmokingAllowed, propertyType, updateHouseCommand.OwnerEmail, updateHouseCommand.OwnerPhoneNumber,
                updateHouseCommand.HouseNo, updateHouseCommand.StreetNo, updateHouseCommand.Area, updateHouseCommand.OwnerName,
                updateHouseCommand.Description, genderRestriction, coordinates.Item1, coordinates.Item2);

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
            House house = _houseRepository.GetHouseById(houseId);
            if (house != null)
            {
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
            IList<House> houses = _houseRepository.GetHouseByOwnerEmail(email, pageNo);
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Get House by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HouseFullRepresentation GetHouseById(string id)
        {
            House house = _houseRepository.GetHouseById(id);
            string dimension = null;
            if (house.Dimension != null)
            {
                dimension = house.Dimension.StringValue + " " + house.Dimension.DimensionType;
            }

            return new HouseFullRepresentation(house.Id, house.Title, house.MonthlyRent, house.NumberOfBedrooms, 
                house.NumberOfKitchens, house.NumberOfBathrooms, house.InternetAvailable, house.LandlinePhoneAvailable,
                house.CableTvAvailable, dimension, house.GarageAvailable, house.SmokingAllowed, house.PropertyType.ToString(), 
                house.OwnerEmail, house.OwnerPhoneNumber, house.Latitude, house.Longitude, house.HouseNo, house.StreetNo, house.Area, 
                house.GetImageList(), house.OwnerName, house.Description, house.GenderRestriction.ToString());
        }

        /// <summary>
        /// Search nearby houses by providing the address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByArea(string address, int pageNo = 0)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);
            // Get 20 coordinates within the range of around 30 kilometers radius
            IList<House> houses = _houseRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2, pageNo);
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Search houses with reference to propertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByPropertyType(string propertyType, int pageNo = 0)
        {
            var convertedPropertyType = (PropertyType)Enum.Parse(typeof(PropertyType), propertyType);
            IList<House> houses = _houseRepository.SearchHousesByPropertyType(convertedPropertyType, pageNo);
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Search nearby houses by providing the area and property type
        /// </summary>
        /// <param name="address"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByAreaAndPropertyType(string address, string propertyType,
            int pageNo = 0)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);
            // Get 20 coordinates within the range of around 30 kilometers radius
            IList<House> houses = _houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinates.Item1, 
                coordinates.Item2, (PropertyType)Enum.Parse(typeof(PropertyType), propertyType), pageNo);
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Get the number of records in the database for the given criteria
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public HouseCountRepresentation GetRecordsCount(string propertyType, string location)
        {
            Tuple<int,int> recordCount = null;
            // If location is not null
            if (!string.IsNullOrWhiteSpace(location))
            {
                // Get the coordinates for the location using the Geocoding API service
                var coordinates = _geocodingService.GetCoordinatesFromAddress(location);
                // And property type is also not null
                if (!string.IsNullOrWhiteSpace(propertyType))
                {
                    var propertyTypeEnum = (PropertyType) Enum.Parse(typeof(PropertyType), propertyType);
                    recordCount = _houseRepository.GetRecordCountByLocationAndPropertyType(coordinates.Item1, coordinates.Item2,
                        propertyTypeEnum);
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
                var propertyTypeEnum = (PropertyType)Enum.Parse(typeof(PropertyType), propertyType);
                recordCount = _houseRepository.GetRecordCountByPropertyType(propertyTypeEnum);
                return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
            }
            // If no criteria is given, return the total number of houses present in the database
            recordCount = _houseRepository.GetTotalRecordCount();
            return new HouseCountRepresentation(recordCount.Item1, recordCount.Item2);
        }

        /// <summary>
        /// Gets all the houses
        /// </summary>
        /// <returns></returns>
        public IList<HousePartialRepresentation> GetAllHouses(int pageNo = 0)
        {
            IList<House> houses = _houseRepository.GetAllHouses(pageNo);
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Get the types of peroperty: Apartment, House, Hostel, Room
        /// </summary>
        /// <returns></returns>
        public IList<string> GetPropertyTypes()
        {
            IList<string> propertyTypeList = Enum.GetNames(typeof(PropertyType)).ToList();
            return propertyTypeList;
        }

        /// <summary>
        /// Add images to an existing house instance
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="imagesList"></param>
        public void AddImagesToHouse(string houseId, IList<string> imagesList)
        {
            House house = _houseRepository.GetHouseById(houseId);
            if (house != null)
            {
                foreach (var imageId in imagesList)
                {
                    house.AddImage(imageId);
                }
                _houseRepository.SaveorUpdate(house);
                _logger.Info("Added images to house successfully. HouseId: {0}", house.Id);
            }
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
        public void DeleteImageFromHouse(string houseId, IList<string> imagesList)
        {
            var house = _houseRepository.GetHouseById(houseId);
            if (house != null)
            {
                foreach (var imageId in imagesList)
                {
                    house.HouseImages.Remove(imageId);
                }
                _houseRepository.SaveorUpdate(house);
                _logger.Info("Deleted images from house successfully. HouseId: {0}", house.Id);
            }
        }

        public bool HouseOwnershipEmailCheck(string houseId, string requesterEmail)
        {
            var house = _houseRepository.GetHouseById(houseId);
            if (house == null)
            {
                throw new NullReferenceException($"No house found for the given HouseId: {houseId}");
            }
            if (house.OwnerEmail.Equals(requesterEmail))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private IList<HousePartialRepresentation> ConvertHouseToRepresentation(IList<House> houses)
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
                        house.MonthlyRent, house.PropertyType.ToString(), house.Dimension, house.NumberOfBedrooms, 
                        house.NumberOfBathrooms, house.NumberOfKitchens, house.OwnerEmail, house.OwnerPhoneNumber,
                        firstImage, house.OwnerName, house.Description);
                    
                    houseRepresentations.Add(houseRepresentation);
                }
            }
            return houseRepresentations;
        }
        
        /// <summary>
        /// Converts the given ImageId to base64String
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        private ImageRepresentation ConvertImageToBase64Representation(string imageId)
        {
            using (Image image = Image.FromFile(HostingEnvironment.MapPath("~/Images/" + imageId)))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, ImageFormatProvider.GetImageFormat());
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    //return Convert.ToBase64String(imageBytes);
                    return new ImageRepresentation(imageId, ImageFormatProvider.GetImageFormat().ToString(), Convert.ToBase64String(imageBytes));
                }
            }
        }
    }
}

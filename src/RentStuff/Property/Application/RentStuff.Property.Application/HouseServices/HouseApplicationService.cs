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
using RentStuff.Common;
using RentStuff.Property.Application.HouseServices.Representation;

namespace RentStuff.Property.Application.HouseServices
{
    /// <summary>
    /// House Application Service
    /// </summary>
    public class HouseApplicationService : IHouseApplicationService
    {
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
                throw new InvalidDataException("Could not find coordinates from the given address");
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
            if (!string.IsNullOrWhiteSpace(createHouseCommand.DimensionType))
            {
                if (string.IsNullOrWhiteSpace(createHouseCommand.DimensionStringValue))
                {
                    throw new NullReferenceException("DimensionStringValue cannot be null if the DimensionType is not null");
                }
                var dimension = new Dimension((DimensionType) Enum.Parse(typeof(DimensionType),
                        createHouseCommand.DimensionType), createHouseCommand.DimensionStringValue,
                    createHouseCommand.DimensionIntValue, house);
                //_houseRepository.SaveorUpdateDimension(dimension);
                house.Dimension = dimension;
            }

            // Save the new house instance
            _houseRepository.SaveorUpdate(house);

            return house.Id;
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
                throw new InstanceNotFoundException(string.Format("House not found for id: {0}", updateHouseCommand.Id));
            }
            // Get the coordinates for the location using the Geocoding API service
            Tuple<decimal, decimal> coordinates = _geocodingService.GetCoordinatesFromAddress(updateHouseCommand.Area);

            if (coordinates == null || coordinates.Item1 == decimal.Zero || coordinates.Item2 == decimal.Zero)
            {
                throw new InvalidDataException("Could not find coordinates from the given address");
            }
            PropertyType propertyType = (PropertyType)Enum.Parse(typeof(PropertyType), updateHouseCommand.PropertyType);
            GenderRestriction genderRestriction =
                (GenderRestriction) Enum.Parse(typeof(GenderRestriction), updateHouseCommand.GenderRestriction);
            
            var dimension = new Dimension(DimensionType.Acre, updateHouseCommand.DimensionStringValue,
                updateHouseCommand.DimensionIntValue, house);
            house.UpdateHouse(updateHouseCommand.Title, updateHouseCommand.MonthlyRent, updateHouseCommand.NumberOfBedrooms,
                updateHouseCommand.NumberOfKitchens, updateHouseCommand.NumberOfBathrooms, updateHouseCommand.InternetAvailable,
                updateHouseCommand.LandlinePhoneAvailable, updateHouseCommand.CableTvAvailable, dimension, updateHouseCommand.GarageAvailable,
                updateHouseCommand.SmokingAllowed, propertyType, updateHouseCommand.OwnerEmail, updateHouseCommand.OwnerPhoneNumber,
                updateHouseCommand.HouseNo, updateHouseCommand.StreetNo, updateHouseCommand.Area, updateHouseCommand.OwnerName,
                updateHouseCommand.Description, genderRestriction, coordinates.Item1, coordinates.Item2);

            _houseRepository.SaveorUpdate(house);
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
            }
            else
            {
                throw new InstanceNotFoundException("No house could be found for the given house id = " + houseId);
            }
        }
        
        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        public IList<HousePartialRepresentation> GetHouseByEmail(string email)
        {
            IList<House> houses = _houseRepository.GetHouseByOwnerEmail(email);
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
            IList<ImageRepresentation> houseImages = new List<ImageRepresentation>();
            foreach (var houseImage in house.HouseImages)
            {
                houseImages.Add(ConvertImageToBase64Representation(houseImage));
            }
            string dimension = null;
            if (house.Dimension != null)
            {
                dimension = house.Dimension.StringValue + " " + house.Dimension.DimensionType;
            }
            return new HouseFullRepresentation(house.Id, house.Title, house.MonthlyRent,
                house.NumberOfBedrooms, house.NumberOfKitchens, house.NumberOfBathrooms, house.InternetAvailable, house.LandlinePhoneAvailable, house.CableTvAvailable,
                dimension, house.GarageAvailable, house.SmokingAllowed,
                house.PropertyType.ToString(), house.OwnerEmail, house.OwnerPhoneNumber, house.Latitude, house.Longitude, house.HouseNo,
                house.StreetNo, house.Area, houseImages, house.OwnerName, house.Description, house.GenderRestriction.ToString());
        }


        /// <summary>
        /// Search nearby houses by providing the address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByArea(string address)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);
            // Get 20 coordinates within the range of around 30 kilometers radius
            IList<House> houses = _houseRepository.SearchHousesByCoordinates(coordinates.Item1, coordinates.Item2);
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Search houses with reference to propertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByPropertyType(string propertyType)
        {
            var convertedPropertyType = (PropertyType)Enum.Parse(typeof(PropertyType), propertyType);
            IList<House> houses = _houseRepository.SearchHousesByPropertyType(convertedPropertyType);
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Search nearby houses by providing the area and property type
        /// </summary>
        /// <param name="address"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public IList<HousePartialRepresentation> SearchHousesByAreaAndPropertyType(string address, string propertyType)
        {
            // Get the coordinates for the location using the Geocoding API service
            var coordinates = _geocodingService.GetCoordinatesFromAddress(address);
            // Get 20 coordinates within the range of around 30 kilometers radius
            IList<House> houses = _houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinates.Item1, 
                coordinates.Item2, (PropertyType)Enum.Parse(typeof(PropertyType), propertyType));
            return ConvertHouseToRepresentation(houses);
        }

        /// <summary>
        /// Gets all the houses
        /// </summary>
        /// <returns></returns>
        public IList<HousePartialRepresentation> GetAllHouses()
        {
            IList<House> houses = _houseRepository.GetAllHouses();
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
            }
        }

        public bool HouseOwnershipEmailCheck(string houseId, string requesterEmail)
        {
            var house = _houseRepository.GetHouseById(houseId);
            if (house == null)
            {
                throw new NullReferenceException("No house found for the given Id");
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
                    ImageRepresentation imageRepresentation = null;
                    string idOfFirstImage = null;
                    IList<string> imageList = house.GetImageList();
                    if (imageList != null && imageList.Count > 0)
                    {
                        idOfFirstImage = imageList[0];
                        if (idOfFirstImage != null)
                        {
                            imageRepresentation = ConvertImageToBase64Representation(idOfFirstImage);
                        }
                    }

                    HousePartialRepresentation houseRepresentation = new HousePartialRepresentation(house.Id, house.Title, house.Area, 
                        house.MonthlyRent, house.PropertyType.ToString(), house.Dimension, house.NumberOfBedrooms, 
                        house.NumberOfBathrooms, house.NumberOfKitchens, house.OwnerEmail, house.OwnerPhoneNumber,
                        imageRepresentation, house.OwnerName, house.Description);
                    
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
                    return  new ImageRepresentation(imageId, ImageFormatProvider.GetImageFormat().ToString(), Convert.ToBase64String(imageBytes));
                }
            }
        }
    }
}

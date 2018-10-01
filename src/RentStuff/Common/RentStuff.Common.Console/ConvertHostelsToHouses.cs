using Ninject;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;

namespace RentStuff.Common.Console
{
    public class ConvertHostelsToHouses
    {
        private static IKernel _kernel;

        public ConvertHostelsToHouses()
        {

        }

        public static void Initialize()
        {
            _kernel = new StandardKernel();

            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            _kernel.Load<PropertyApplicationNinjectModule>();
            IResidentialPropertyRepository residentialPropertyRepository = _kernel.Get<IResidentialPropertyRepository>();
            var properties = residentialPropertyRepository.GetAllHostels();
            System.Console.WriteLine("Starting conversion from Hostel to House. Count = {0}", properties.Count);
            foreach (var property in properties)
            {
                var hostel = (Hostel)property;
                if (hostel.PropertyType == RentStuff.Common.Utilities.Constants.Hostel)
                {
                    var house = new House(hostel.Title, hostel.RentPrice, 1, 0, 1, 
                        hostel.InternetAvailable, false, hostel.CableTvAvailable, null,
                        hostel.ParkingAvailable, true, Utilities.Constants.Hostel, hostel.OwnerEmail, hostel.OwnerPhoneNumber,
                        hostel.Latitude, hostel.Longitude, null, null, hostel.Area, hostel.OwnerName,
                        hostel.Description, hostel.GenderRestriction, hostel.IsShared, hostel.RentUnit,
                        hostel.LandlineNumber, hostel.Fax, hostel.AC, hostel.Geyser, hostel.Balcony,
                        hostel.Lawn, hostel.CctvCameras, hostel.BackupElectricity, hostel.Heating,
                        false, hostel.Elevator);
                    foreach (var houseImage in hostel.Images)
                    {
                        house.AddImage(houseImage);
                    }
                    residentialPropertyRepository.SaveorUpdate(house);
                    residentialPropertyRepository.Delete(hostel);
                    System.Console.WriteLine("Hostel {0} converted to House {1}",
                        hostel.Id, house.Id);
                }
            }
            System.Console.WriteLine("Finished converting Hostels to house");
        }
    }
}

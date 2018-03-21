using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ninject;
using RentStuff.Common.NHibernate.Wrappers;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Application.PropertyServices;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Property.Infrastructure.Persistence.Repositories;

namespace RentStuff.Common.Console
{
    public class ConvertHouseToHostel
    {
        private static IKernel _kernel;

        public ConvertHouseToHostel()
        {
            
        }

        public static void Initialize()
        {
            _kernel = new StandardKernel();

            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            _kernel.Load<PropertyApplicationNinjectModule>();
            IResidentialPropertyRepository residentialPropertyRepository = _kernel.Get<IResidentialPropertyRepository>();
            var properties = residentialPropertyRepository.GetAllProperties();
            foreach (var property in properties)
            {
                var house = (House) property;
                if (house.PropertyType == RentStuff.Common.Utilities.Constants.Hostel)
                {
                    var hostel = new Hostel(house.Title, house.RentPrice, house.OwnerEmail, house.OwnerPhoneNumber,
                        house.Latitude, house.Longitude, house.Area, house.OwnerName, house.Description,
                        house.GenderRestriction, house.IsShared, house.RentUnit, house.InternetAvailable,
                        house.CableTvAvailable, house.GarageAvailable, Utilities.Constants.Hostel, false,
                        false,false,false,false,false,false,false,false,false,false,false,false,0,"","",false);

                    foreach (var houseImage in house.Images)
                    {
                        hostel.AddImage(houseImage);
                    }
                    residentialPropertyRepository.SaveorUpdate(hostel);
                    residentialPropertyRepository.Delete(house);
                }
            }
        }
    }
}

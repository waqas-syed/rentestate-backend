using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Common.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //IdentityBcAuthenticationTests.InitializeTests();
            CreateHouseTestData.Initialize();
        }

       
    }
}

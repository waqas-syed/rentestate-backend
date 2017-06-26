using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Common.Console
{
    public class CreateHouseTestData
    {
        private static HttpClient _httpClient = Utility.InitializeHttpClient();
        public static void Initialize()
        {
            PostHouse();
            System.Console.ReadLine();
        }

        static async void PostHouse()
        {
            IList<string> areas = new List<string>()
            {
                "Pindora, Rawalpindi, Pakistan", "Satellite Town, Rawalpindi, Pakistan", "Saddar, Rawalpindi, Pakistan",
                "6th Rd, Rawalpindi, Pakistan", "I-9, Islamabad, Pakistan", "I-8, Islamabad, Pakistan",
                "F-7, Islamabad, Pakistan", "Commercial Market Rd, Rawalpindi, Pakistan",
                "The Centaurus Mall, Jinnah Avenue, Islamabad, Pakistan", "Beor, Pakistan",
                "Bahria Town, Rawalpindi, Pakistan", "DHA Phase II, Pakistan", "Raja Bazar, Rawalpindi, Pakistan",
                "Stadium Rd, Rawalpindi, Pakistan", "E-11, Islamabad, Pakistan",
                "Chaklala Scheme 3, Rawalpindi, Pakistan", "Blue Area, Islamabad, Pakistan"
            };

            for (int i = 0; i < areas.Count; i++)
            {
                // Saving House # 1 - SET 1: Should appear in search results
                string title = "Title # " + i;
                int rent = 100000 + i;
                string ownerEmail = "house@1234567-" + i + ".com";
                string ownerPhoneNumber = "+92500100000" + i;
                if (i > 9)
                {
                    ownerPhoneNumber = "+9250010000" + i;
                }
                string houseNo = "House # " + i;
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend." + i;
                string genderRestriction = GenderRestriction.BoysOnly.ToString();
                string streetNo = i.ToString();
                int numberOfBathrooms = i;
                int numberOfBedrooms = i;
                int numberOfKitchens = i;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = false;
                bool internetAvailable = false;
                bool landlinePhoneAvailable = false;
                bool cableTvAvailable = false;
                bool garageAvailable = false;
                bool smokingAllowed = false;
                string propertyType = "House";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = i.ToString();
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name " + i;
                if (i % 2 == 0)
                {
                    dimensionType = DimensionType.Kanal.ToString();
                    familiesOnly = true;
                }
                else
                {
                    dimensionType = DimensionType.Marla.ToString();
                    boysOnly = true;
                    internetAvailable = true;
                    landlinePhoneAvailable = true;
                    cableTvAvailable = true;
                    garageAvailable = true;
                    smokingAllowed = true;
                }

                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, areas[i], dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction);
                //JsonConvert.SerializeObject(house);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("v1/house", house);
                response.EnsureSuccessStatusCode();

                var responseAsObject = await response.Content.ReadAsAsync(typeof(string));
                string responseBodyAsText = responseAsObject.ToString();
                //responseBodyAsText = responseBodyAsText.Replace("<br>", Environment.NewLine); // Insert new lines
                string houseId = responseBodyAsText.Replace(@"\""", "");
                System.Console.WriteLine(houseId);

                System.Console.WriteLine("Insertion Completed: House # " + i + " | Id = " + houseId);

                string file = @"C:\Pics\Wallpapers\Houses\" + i + ".jpg";
                var fileStream = File.Open(file, FileMode.Open);
                var fileInfo = new FileInfo(file);
                bool _fileUploaded = false;

                var content = new MultipartFormDataContent();
                //content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                //content.Headers.Add("Content-Type", "multipart");
                content.Headers.Add("HouseId", houseId);
                content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", fileInfo.Name));

                Task taskUpload = _httpClient.PostAsync("v1/HouseImageUpload", content).ContinueWith(task =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            System.Console.WriteLine("Image Upload completed: House # " + i + " | Id = " + houseId);
                        }
                        else
                        {
                            Debug.WriteLine("Status Code: {0} - {1}", response.StatusCode, response.ReasonPhrase);
                            Debug.WriteLine("Response Body: {0}", response.Content.ReadAsStringAsync().Result);
                        }
                    }

                    fileStream.Dispose();
                });
                //System.Console.WriteLine("Task Completed: House # " + i + " | Id = " + houseId);

            }
            System.Console.WriteLine("All tasks completed!!! Toast");

            System.Console.ReadLine();
        }
    }
}

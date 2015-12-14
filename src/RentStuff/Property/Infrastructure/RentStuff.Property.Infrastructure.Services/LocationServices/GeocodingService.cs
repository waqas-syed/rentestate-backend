using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using RentStuff.Property.Domain.Model.Services;

namespace RentStuff.Property.Infrastructure.Services.LocationServices
{
    /// <summary>
    /// Service responsible for interacting with the Google Geocoding API to get the co-ordinates given the location
    /// </summary>
    public class GeocodingService : IGeocodingService
    {
        /// <summary>
        /// Gets the corordinates given the address
        /// </summary>
        /// <returns></returns>
        public Tuple<decimal, decimal> GetCoordinatesFromAddress(string address)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:9000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = client.GetAsync("https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + ConfigurationManager.AppSettings.Get("GeocodingApiKey")).Result;
                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    dynamic coordinates = JObject.Parse(responseString);
                    return new Tuple<decimal, decimal>((decimal)coordinates.results[0].geometry.location.lat.Value, (decimal)coordinates.results[0].geometry.location.lng.Value);
                }
            }
            return null;
        }
    }
}

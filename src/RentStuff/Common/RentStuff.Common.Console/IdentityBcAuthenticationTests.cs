using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RentStuff.Common.Console.DTOs;

namespace RentStuff.Common.Console
{
    /// <summary>
    /// Tests the identity Bounded Context as a separate Console appication by making REST calls and asserting the returned values
    /// using nUnit to evaluate
    /// </summary>
    public class IdentityBcAuthenticationTests
    {
        private static HttpClient _httpClient = Utility.InitializeHttpClient();

        public static void InitializeTests()
        {
            AuthenticationStart();
            System.Console.ReadLine();
        }

        static async void AuthenticationStart()
        {
            var email = "waqas.shah.syed@gmail.com";
            var name = "Gandalf The Grey";
            var password = "TheStaff123!";
            var confirmPassword = "TheStaff123!";
            CreateUserDto createUserCommand = new CreateUserDto();
            createUserCommand.Email = email;
            createUserCommand.FullName = name;
            createUserCommand.Password = password;
            createUserCommand.ConfirmPassword = confirmPassword;
            try
            {
                // ------------Succesful Registration------------------ //
                System.Console.WriteLine("Registration Process Starting");
                HttpResponseMessage registerResponse = await _httpClient.PostAsJsonAsync("api/account/register", createUserCommand);
                registerResponse.EnsureSuccessStatusCode();
                Assert.AreEqual(System.Net.HttpStatusCode.OK, registerResponse.StatusCode);
                System.Console.WriteLine("Registration Successful");
                
                // ------------Successful Login ------------------ //
                System.Console.WriteLine("Login Process Starting");
                
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpContent loginRequestContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    // username and Email are same. Email is used as username
                    new KeyValuePair<string, string>("username", email),
                    new KeyValuePair<string, string>("password", password)
                });
                HttpResponseMessage loginResponse = await _httpClient.PostAsync("token", loginRequestContent);
                loginResponse.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
                var loginResponseContent = loginResponse.Content.ReadAsStringAsync().Result;
                Assert.IsFalse(string.IsNullOrWhiteSpace(loginResponseContent));
                var loginResponseKeyValuePair = JsonConvert.DeserializeObject<Dictionary<string, string>>(loginResponseContent);
                Assert.AreEqual("access_token", loginResponseKeyValuePair.First().Key);
                Assert.IsFalse(string.IsNullOrWhiteSpace(loginResponseKeyValuePair.First().Value));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponseKeyValuePair.First().Value);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("Error: " + exception.Message);
                //throw;
            }
        }
    }
}

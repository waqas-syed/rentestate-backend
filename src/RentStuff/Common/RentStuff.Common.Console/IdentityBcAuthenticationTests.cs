using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
                System.Console.WriteLine("Registration Process Starting");
                HttpResponseMessage registerResponse = await _httpClient.PostAsJsonAsync("api/account/register", createUserCommand);
                //HttpResponseMessage response = await _httpClient.PostAsJsonAsync("v1/house", house);
                registerResponse.EnsureSuccessStatusCode();
                Assert.AreEqual(System.Net.HttpStatusCode.OK, registerResponse.StatusCode);
                System.Console.WriteLine("Registration Successful");

                System.Console.WriteLine("Login Process Starting");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("x-www-form-urlencoded"));
                HttpResponseMessage loginResponse = await _httpClient.PostAsync("token", null);
                loginResponse.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("Error: " + exception.Message);
                //throw;
            }
        }
    }
}

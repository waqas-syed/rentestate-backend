using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace RentStuff.Common.Domain.Model
{
    /// <summary>
    /// Performs assertions on the values provided
    /// </summary>
    public class Assertion
    {
        /// <summary>
        /// Checks that the given object is not null
        /// </summary>
        /// <param name="currentObject"></param>
        public static void AssertNotNull(object currentObject)
        {
            if (currentObject == null)
            {
                throw new NullReferenceException("Given object is null");
            }
        }

        /// <summary>
        /// Checks that the given string is not null or empty
        /// </summary>
        /// <param name="currentString"></param>
        public static void AssertStringNotNullorEmpty(string currentString)
        {
            if (string.IsNullOrEmpty(currentString))
            {
                throw new NullReferenceException("Given string is null or empty");
            }
        }

        /// <summary>
        /// Checks that the given number is not zero
        /// </summary>
        /// <param name="currentNumber"></param>
        public static void AssertNumberNotZero(long currentNumber)
        {
            if (currentNumber == 0)
            {
                throw new NullReferenceException("Given nuumber is 0");
            }
        }

        /// <summary>
        /// Checks that the given number is not zero
        /// </summary>
        /// <param name="currentNumber"></param>
        public static void AssertDecimalNotZero(decimal currentNumber)
        {
            if (currentNumber == Decimal.Zero)
            {
                throw new NullReferenceException("Given nuumber is 0");
            }
        }

        /// <summary>
        /// Checks if the given email is in a valid format. Throws exception if it is not.
        /// </summary>
        /// <param name="emailAddress"></param>
        public static void IsEmailValid(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Email is not in a valid format");
            }
        }

        /// <summary>
        /// Check if the given phone number is in a valid format. Throws exception if it is not
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static void IsPhoneNumberValid(string phoneNumber)
        {
            Regex regex = new Regex(@"^\d{11}$");
            Match match = regex.Match(phoneNumber);
            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid phone number");
            }
        }
    }
}

using System;

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
    }
}

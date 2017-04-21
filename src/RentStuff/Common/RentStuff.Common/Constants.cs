
using System.Configuration;

namespace RentStuff.Common
{
    public class Constants
    {
        public const string CompanyName = "Zarqoon";
        //public const string FrontEndUrl = "http://localhost:11803";
        public static readonly string FrontEndUrl = ConfigurationManager.AppSettings.Get("FrontEndUrl");
        public const string AccountActivationUrlLocation = "index.html#/activate-account";
        public const string PasswordResetUrlLocation = "index.html#/reset-password";
        public const string HOUSEIMAGESDIRECTORY = "~/Images/";
    }
}

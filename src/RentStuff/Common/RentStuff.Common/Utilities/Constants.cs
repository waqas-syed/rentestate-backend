
using System.Configuration;

namespace RentStuff.Common.Utilities
{
    public class Constants
    {
        public const string CompanyName = "Zarqoon";
        public static readonly string FrontEndUrl = ConfigurationManager.AppSettings.Get("FrontEndUrl");
        public static readonly string FacebookRedirectUri = FrontEndUrl + ConfigurationManager.AppSettings.Get("FacebookRedirectUri");
        public static readonly string FrontendClientId = "zarqoon-frontend";
        public static readonly string FacebookAppId = "114619729160977";
        public static readonly string FacebookAppSecret = "6cb55f27007c81daa0dad0f88133a938";
        public static readonly string FacebookAcccessToken = "114619729160977|0lq_SsLcffSnGs5XWN70elTv77E";
        public static readonly string FacebookEmailScope = "email";
        public static readonly string FacebookUserInformationEndpoint = "https://graph.facebook.com/v2.8/me?fields=id,name,email";
        public const string AccountActivationUrlLocation = "activate-account";
        public const string PasswordResetUrlLocation = "reset-password";
    }
}

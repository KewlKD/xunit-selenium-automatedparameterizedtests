using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace WeBrick.Test.UI
{
    public static class Configuration
    {
        internal static string TestEnvironment { get; } = Config["appSettings:testEnvironment"].ToLower();
        internal static string BuyerDirectoryConnectionString { get; } = Config[$"appSettings:{TestEnvironment}:buyerDirectoryConnectionString"];
        internal static string LocalConnectionString { get; } = Config[$"appSettings:{TestEnvironment}:connectionString"];
        internal static string AuthConnectionString { get; } = Config[$"appSettings:{TestEnvironment}:authConnectionString"];
        internal static string Website { get; } = Config[$"appSettings:{TestEnvironment}:website"];
        internal static string RegistrationPage { get; } = Config[$"appSettings:{TestEnvironment}:registrationPage"];
        internal static string BackOffice { get; } = Config[$"appSettings:{TestEnvironment}:backOffice"];
        internal static string UserClaimPage { get; } = Config[$"appSettings:{TestEnvironment}:userClaimPage"];
        internal static string Api { get; } = Config[$"appSettings:{TestEnvironment}:api"];
        internal static string LoginUrl { get; } = Config[$"appSettings:{TestEnvironment}:loginUrl"];
        internal static string LogoutUrl { get; } = Config[$"appSettings:{TestEnvironment}:logoutUrl"];
        internal static string PhoneNumber { get; } = Config[$"appSettings:{TestEnvironment}:phoneNumber"];
        internal static string Password { get; } = Config[$"appSettings:{TestEnvironment}:password"];
        internal static string MainPhoneNumber { get; } = Config[$"appSettings:{TestEnvironment}:mainPhoneNumber"];
        internal static string MainPassword { get; } = Config[$"appSettings:{TestEnvironment}:mainPassword"];
        internal static string AuthUrl { get; } = Config[$"appSettings:{TestEnvironment}:authUrl"];
        internal static bool RunHeadless { get; } = Config[$"appSettings:{TestEnvironment}:runHeadless"].ToLower() == "true";
        internal static string FooterElementZero { get; } = Config[$"appSettings:{TestEnvironment}:footerElementZero"].ToLower();
        internal static string FooterElementOne { get; } = Config[$"appSettings:{TestEnvironment}:footerElementOne"].ToLower();
        internal static string FooterElementTwo { get; } = Config[$"appSettings:{TestEnvironment}:footerElementTwo"].ToLower();
        internal static string FooterElementThree { get; } = Config[$"appSettings:{TestEnvironment}:footerElementThree"].ToLower();
        internal static string FooterElementFour { get; } = Config[$"appSettings:{TestEnvironment}:footerElementFour"].ToLower();
        internal static string FooterElementFive { get; } = Config[$"appSettings:{TestEnvironment}:footerElementFive"].ToLower();
        internal static string FooterText { get; } = Config[$"appSettings:{TestEnvironment}:footerText"];
        internal static string SampleProperty1 { get; } = Config[$"appSettings:{TestEnvironment}:sampleProperty1"];
        internal static string SocialMediaElementZero { get; } = Config[$"appSettings:{TestEnvironment}:socialMediaElementZero"].ToLower();
        internal static string SocialMediaElementOne { get; } = Config[$"appSettings:{TestEnvironment}:socialMediaElementOne"].ToLower();
        internal static string SocialMediaElementTwo { get; } = Config[$"appSettings:{TestEnvironment}:socialMediaElementTwo"].ToLower();
        internal static string SampleProperty { get; } = Config[$"appSettings:{TestEnvironment}:sampleProperty"];
        internal static string ChromeDriverUrl { get; } = Config[$"appSettings:chromeDriverUrl"];


        private static IConfigurationRoot Config
        {
            get
            {
                if (ConfigField == null)
                {
                    ConfigField = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                }
                return ConfigField;
            }
        }
        private static IConfigurationRoot ConfigField;

        public static string WebsiteName
        {
            get
            {
                string websiteName = Configuration.Website;
                if (websiteName.IndexOf("@") >= 0)
                {
                    websiteName = websiteName.Substring(websiteName.IndexOf("@") + 1);
                }
                List<string> parts = websiteName.Split(new char[] { '/', '.' }).ToList();
                parts.RemoveAll(p => string.IsNullOrWhiteSpace(p));
                websiteName = parts[^2];
                return websiteName;
            }
        }

        public static string BackOfficeName
        {
            get
            {
                string websiteName = Configuration.BackOffice;

                if (websiteName.IndexOf("@") >= 0)
                {
                    websiteName = websiteName.Substring(websiteName.IndexOf("@") + 1);
                }
                List<string> parts = websiteName.Split(new char[] { '/', '.' }).ToList();
                parts.RemoveAll(p => string.IsNullOrWhiteSpace(p));
                websiteName = parts[^2];
                return websiteName;
            }
        }

        public static string GetOptionalValue(string environment, string key)
        {
            return Config[$"appSettings:{environment}:{key}"].ToString();
        }
    }
}

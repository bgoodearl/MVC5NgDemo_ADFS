using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MVCDemo
{
    public class Startup
    {
        internal static class StTags
        {
            internal const string adfs_MetadataAddress = "ast:adfsMetadataAddress";
            internal const string realmADFSId = "ast:realmADFSId";
        }
        internal static class Config
        {
            private static string _adfs_MetadataAddress;
            internal static string ADFS_MetadataAddress
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_adfs_MetadataAddress))
                    {
                        _adfs_MetadataAddress = ConfigurationManager.AppSettings[StTags.adfs_MetadataAddress];
                    }
                    return _adfs_MetadataAddress;
                }
            }
            private static string _adfs_RealmId;
            internal static string ADFSRealmId
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_adfs_RealmId))
                    {
                        _adfs_RealmId = ConfigurationManager.AppSettings[StTags.realmADFSId];
                    }
                    return _adfs_RealmId;
                }
            }
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions()
            {
                AuthenticationType = "Cookie",
                CookieName = "M5NgAuth",
                CookiePath = "/demo/"
            });
            app.UseWsFederationAuthentication(new Microsoft.Owin.Security.WsFederation.WsFederationAuthenticationOptions() {
                AuthenticationType = "WSFed",
                SignInAsAuthenticationType = "Cookie",
                MetadataAddress = Config.ADFS_MetadataAddress, //(ADFS Metadata URL)
                Wtrealm = Config.ADFSRealmId, //Relying Party (id in ADFS)
            });
        }
    }
}
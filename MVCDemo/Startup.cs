using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MVCDemo
{
    public class Startup
    {
        internal static class StTags
        {
            internal const string adfs_MetadataURL = "ast:adfsMetadataURL";
            internal const string adfs_RealmId = "ast:adfsRealmId";
            internal const string adfs_URL_adfs = "ast:adfsURLadfs";
            internal const string adfs_WebApiClientId = "ast:adfsWebApiClientId";
            internal const string appDomain = "ast:appDomain";
            internal const string loginCallbackUrl = "ast:loginCallbackUrl";
            internal const string webApiADFSId = "ast:webApiADFSId";
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
                        _adfs_MetadataAddress = ConfigurationManager.AppSettings[StTags.adfs_MetadataURL];
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
                        _adfs_RealmId = ConfigurationManager.AppSettings[StTags.adfs_RealmId];
                    }
                    return _adfs_RealmId;
                }
            }

            private static string _adfs_URL_adfs;
            internal static string ADFS_URL_adfs
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_adfs_URL_adfs))
                    {
                        _adfs_URL_adfs = ConfigurationManager.AppSettings[StTags.adfs_URL_adfs];
                    }
                    return _adfs_URL_adfs;
                }
            }

            private static string _adfs_WebApiClientId;
            internal static string ADFSWebApiClientId
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_adfs_WebApiClientId))
                    {
                        _adfs_WebApiClientId = ConfigurationManager.AppSettings[StTags.adfs_WebApiClientId];
                    }
                    return _adfs_WebApiClientId;
                }
            }

            private static string _appDomain;
            internal static string AppDomain
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_appDomain))
                    {
                        _appDomain = ConfigurationManager.AppSettings[StTags.appDomain];
                    }
                    return _appDomain;
                }
            }

            private static string _loginCallbackUrl;
            internal static string LoginCallbackUrl
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_loginCallbackUrl))
                    {
                        _loginCallbackUrl = ConfigurationManager.AppSettings[StTags.loginCallbackUrl];
                    }
                    return _loginCallbackUrl;
                }
            }

            private static string _webApiADFSId;
            internal static string WebApiADFSId
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_webApiADFSId))
                    {
                        _webApiADFSId = ConfigurationManager.AppSettings[StTags.webApiADFSId];
                    }
                    return _webApiADFSId;
                }
            }

            internal const string RedirectCookieName = "MVC5NgDemoRedir";
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
                //Added redirect to get token for Angular app
                Notifications = new Microsoft.Owin.Security.WsFederation.WsFederationAuthenticationNotifications()
                {
                    SecurityTokenValidated = ctx =>
                    {
                        var callbackUri = new Uri(Config.LoginCallbackUrl);
                        var ctx2 = new AuthenticationContext(Config.ADFS_URL_adfs, false);
                        var url = ctx2.GetAuthorizationRequestURL(Config.WebApiADFSId, Startup.Config.ADFSWebApiClientId, callbackUri,
                            Microsoft.IdentityModel.Clients.ActiveDirectory.UserIdentifier.AnyUser, null);
                        //Add cookie to tell /demo/auth/loggedin where to redirect
                        if (!string.IsNullOrWhiteSpace(ctx.AuthenticationTicket.Properties.RedirectUri))
                        {
                            //TODO: protect cookie
                            ctx.Response.Cookies.Append(Config.RedirectCookieName, ctx.AuthenticationTicket.Properties.RedirectUri, new Microsoft.Owin.CookieOptions()
                                {
                                    Domain = Config.AppDomain,
                                    Expires = DateTime.UtcNow.AddMinutes(30),
                                    Path = "/demo/"
                                });
                        }
                        ctx.AuthenticationTicket.Properties.RedirectUri = url.AbsoluteUri;
                        return Task.FromResult(0);
                    }
                }
            });
            app.UseActiveDirectoryFederationServicesBearerAuthentication(
                new Microsoft.Owin.Security.ActiveDirectory.ActiveDirectoryFederationServicesBearerAuthenticationOptions()
                {
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidAudience = Config.WebApiADFSId
                    },
                    MetadataEndpoint = Config.ADFS_MetadataAddress
                });
        }

    }
}
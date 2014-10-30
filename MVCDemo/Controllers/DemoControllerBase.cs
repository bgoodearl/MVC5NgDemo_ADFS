using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSC = System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Text;
using MVCDemo.Infrastructure;
using BGoodMusic.EFDAL.Interfaces;

namespace MVCDemo.Controllers
{
    public class DemoControllerBase : Controller
    {

        protected Guid GetTokenIdFromCookie(string cookieName, string appName)
        {
            if (Request.Cookies.AllKeys.Contains(cookieName))
            {
                var cookie = Request.Cookies[cookieName];
                if (cookie != null)
                {
                    byte[] guidBytes = MachineKeyHelper.Unprotect(cookie.Value, this.User, appName);
                    Guid guid = new Guid(guidBytes);
                    return guid;
                }
            }
            return Guid.Empty;
        }

        protected void RemoveCookie(string cookieName)
        {
            HttpCookie currentCookie = Request.Cookies[cookieName];
            if (currentCookie != null)
            {
                Response.Cookies.Remove(cookieName);
                currentCookie.Expires = DateTime.Now.AddDays(-10);
                currentCookie.Value = null;
                Response.SetCookie(currentCookie);
            }
        }

        protected StringBuilder ReportException(Exception ex, StringBuilder sb)
        {
            sb.AppendFormat("{1}: {2}{0}", Environment.NewLine, ex.GetType().Name, ex.Message);
            if (ex.InnerException != null)
            {
                Exception ex2 = ex.InnerException;
                sb.AppendFormat("{1}: {2}{0}",
                    Environment.NewLine, ex2.GetType().Name, ex2.Message);
                if (ex2.InnerException != null)
                {
                    ex2 = ex2.InnerException;
                    sb.AppendFormat("{1}: {2}{0}",
                        Environment.NewLine, ex2.GetType().Name, ex2.Message);
                }
            }
            return sb;
        }

        protected StringBuilder ReportException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            return ReportException(ex, sb);
        }

        protected string RefreshToken(string cookieName, string cookieProtectionApp, string tokenProtectionApp)
        {
            Guid tokenId = GetTokenIdFromCookie(cookieName, cookieProtectionApp);
            if (!Guid.Empty.Equals(tokenId))
            {
                using (IBGoodMusicRepository repo = new BGoodMusic.EFDAL.BGoodMusicDBContext())
                {
                    var userInfo = repo.GetUserInfoItem(tokenId);
                    if (userInfo != null)
                    {
                        string token = MachineKeyHelper.UnprotectTo1252String(userInfo.Token, this.User, tokenProtectionApp);
                        return token;
                    }
                }
            }
            return null;
        }

        protected void CleanupForLogout(string cookieName, string cookieProtectionApp)
        {
            Guid tokenId = GetTokenIdFromCookie(cookieName, cookieProtectionApp);
            if (!Guid.Empty.Equals(tokenId))
            {
                using (IBGoodMusicRepository repo = new BGoodMusic.EFDAL.BGoodMusicDBContext())
                {
                    repo.RemoveUserInfoItem(tokenId);
                }
                RemoveCookie(cookieName);
            }
        }

        protected string GetAccessToken(string tokenIdCookieName,
            string cookieProtectionApp,
            string tokenProtectionApp,
            string adfsAuthUserId,
            bool verbose,
            StringBuilder msg)
        {
            string refreshToken = RefreshToken(tokenIdCookieName, cookieProtectionApp, tokenProtectionApp);
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var ctx = new AuthenticationContext(Startup.Config.ADFS_URL_adfs, false);
                var cred = new ClientCredential(adfsAuthUserId, "NotASecret");
                var response = ctx.AcquireTokenByRefreshToken(refreshToken, cred);
                if (response == null)
                {
                    msg.AppendLine("no response to token from refresh token request.");
                }
                else if (string.IsNullOrWhiteSpace(response.AccessToken))
                {
                    msg.AppendLine("no access token from refresh token request.");
                }
                else
                {
                    if (verbose)
                    {
                        msg.AppendFormat("Got access token from refresh token.{0}... len = {1}, start = \"{2}\"{0}",
                            Environment.NewLine,
                            response.AccessToken.Length,
                            response.AccessToken.Substring(0, 20));
                    }
                    return response.AccessToken;
                }
            }
            else
            {
                ViewBag.Message = "No Refresh Token";
            }
            return null;
        }

        protected bool GetRefreshTokenAndSave(string code,
            string adfsAuthUserId,
            Uri callbackUri,
            string cookieName,
            string protectionApp,
            StringBuilder msg)
        {
            var ctx = new AuthenticationContext(Startup.Config.ADFS_URL_adfs, false);
            var cred = new ClientCredential(adfsAuthUserId, "NotASecret");
            var response = ctx.AcquireTokenByAuthorizationCode(code, callbackUri, cred);
            if (response == null)
            {
                msg.AppendLine("Response null");
            }
            else
            {
                msg.AppendLine("Got response");
                if (!string.IsNullOrWhiteSpace(response.AccessToken))
                    msg.AppendLine(" - Got Access Token");
                if (string.IsNullOrWhiteSpace(response.RefreshToken))
                {
                    msg.AppendLine(" - No Refresh Token");
                }
                else
                {
                    string nameId = null;
                    string protectedToken = null;
                    Guid tokenId = Guid.Empty;
                    msg.AppendFormat(" - Got Refresh Token len={1}{0} -- starts with \"{2}\"{0}",
                        Environment.NewLine,
                        response.RefreshToken.Length,
                        response.RefreshToken.Substring(0, 10));
                    SSC.ClaimsPrincipal cp = this.User as SSC.ClaimsPrincipal;
                    if (cp != null)
                    {
                        nameId = cp.GetNameIdentiferValue();
                        if (!string.IsNullOrWhiteSpace(nameId))
                        {
                            protectedToken = MachineKeyHelper.Protect1252(response.RefreshToken, cp, protectionApp);
                            string unprotectedToken = MachineKeyHelper.UnprotectTo1252String(protectedToken, this.User, protectionApp);
                            if (response.RefreshToken != unprotectedToken)
                            {
                                msg.AppendFormat(" - Protect / Unprotect different.{0}... token len = {1}, start=\"{2}\"{0}... token len = {3}, start = \"{4}\"{0}",
                                    Environment.NewLine,
                                    response.RefreshToken.Length,
                                    response.RefreshToken.Substring(0, 20),
                                    unprotectedToken.Length,
                                    unprotectedToken.Substring(0, 20));
                            }
                            if (string.IsNullOrWhiteSpace(nameId))
                                msg.Append(" ** could not get Name Identifier **");
                            if (string.IsNullOrWhiteSpace(protectedToken))
                                msg.Append(" ** could not protect token **");

                            if (!string.IsNullOrWhiteSpace(nameId))
                            {
                                using (IBGoodMusicRepository repo = new BGoodMusic.EFDAL.BGoodMusicDBContext())
                                {
                                    tokenId = repo.AddNewUserInfo(nameId, protectedToken);
                                    if (Guid.Empty.Equals(tokenId))
                                        msg.Append(" ** failed to create UserInfo **");
                                }
                            }
                            if (!Guid.Empty.Equals(tokenId))
                            {
                                var tokenIdCookie = new HttpCookie(cookieName)
                                {
                                    Domain="localhost",
                                    HttpOnly = true,
                                    Path = "/demo/",
                                    Secure = true,
                                    Value = MachineKeyHelper.Protect(tokenId.ToByteArray(), cp, protectionApp)
                                };
                                Response.Cookies.Add(tokenIdCookie);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

    }
}
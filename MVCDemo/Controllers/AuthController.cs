using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MVCDemo.Infrastructure;

namespace MVCDemo.Controllers
{
    [RoutePrefix("demo/auth")]
    [Route("{action}")]
    public class AuthController : DemoControllerBase
    {
        // GET: Auth
        [AllowAnonymous]
        public ActionResult Login()
        {
            return RedirectToAction("LoggedIn");
        }

        public ActionResult LoggedIn()
        {
            return View();
        }

        public void Logout()
        {
            CleanupForLogout(CommonDefs.Constants.CookieName_RefreshTokenId, CommonDefs.Constants.ProtectionAppName);
            var ctx = Request.GetOwinContext();
            ctx.Authentication.SignOut();
        }

        protected string CallbackUrl
        {
            get
            {
                var url = string.Format("{0}://{1}{2}",
                    Request.Url.Scheme, Request.Url.Authority, Url.Content("~/demo/auth/loggedin2")).ToLower();
                return url;
            }
        }

        protected string GetRedirectUrlFromCookie(string cookieName)
        {
            if (Request.Cookies.AllKeys.Contains(cookieName))
            {
                var redirectCookie = Request.Cookies[cookieName];
                //TODO: validate protected cookie
                if ((redirectCookie != null) && !string.IsNullOrWhiteSpace(redirectCookie.Value))
                {
                    string rawUrl = System.Net.WebUtility.UrlDecode(redirectCookie.Value);
                    Uri redirectUri = new Uri(rawUrl);
                    if (redirectUri.DnsSafeHost.ToLower() == this.Request.Url.DnsSafeHost.ToLower())
                        return redirectUri.AbsolutePath;
                }
            }
            return null;
        }

        public ActionResult LoggedIn2(string code, string error)
        {
            StringBuilder msg = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(error))
            {
                msg.AppendFormat("Error: {0}", error);
            }
            else if (string.IsNullOrWhiteSpace(code))
            {
                msg.Append("no code");
            }
            else
            {
                var callbackUri = new Uri(CallbackUrl);
                try
                {
                    string redirectUrl = GetRedirectUrlFromCookie(Startup.Config.RedirectCookieName);
                    RemoveCookie(Startup.Config.RedirectCookieName);

                    if (GetRefreshTokenAndSave(code, Startup.Config.ADFSWebApiClientId,
                        callbackUri, CommonDefs.Constants.CookieName_RefreshTokenId,
                        CommonDefs.Constants.ProtectionAppName, msg))
                    {
                        if (!string.IsNullOrWhiteSpace(redirectUrl))
                            return Redirect(redirectUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                catch (AdalServiceException ex)
                {
                    if (msg.Length > 0)
                        msg.AppendLine(" -- ");
                    msg.AppendFormat("uri=\"{1}\"{0}", Environment.NewLine, callbackUri);
                    ReportException(ex, msg);
                }
                catch (Exception ex)
                {
                    if (msg.Length > 0)
                        msg.AppendLine(" -- ");
                    ReportException(ex, msg);
                }
            }
            ViewBag.Message = msg.ToString();
            return View();
        }

    }
}
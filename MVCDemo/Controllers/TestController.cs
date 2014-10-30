using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SSC = System.Security.Claims;
using MVCDemo.Infrastructure;
using MVCDemo.ViewModels.Test;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MVCDemo.ViewModels.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MVCDemo.Controllers
{
    [RoutePrefix("demo/test")]
    [Route("{action}")]
    public class TestController : DemoControllerBase
    {
        // GET: Test
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AngularTest()
        {
            AngularTest model = new ViewModels.Test.AngularTest();

            StringBuilder msg = new StringBuilder();
            try
            {
                string token = GetAccessToken(CommonDefs.Constants.CookieName_RefreshTokenId,
                    CommonDefs.Constants.ProtectionAppName, //Cookie Protection App
                    CommonDefs.Constants.ProtectionAppName, //Token Protection App
                    Startup.Config.ADFSWebApiClientId,
                    true, /*verbose*/
                    msg);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    TokenInfo ti = new TokenInfo {
                        Tk = token
                    };
                    model.JsonToken = JsonConvert.SerializeObject(ti,
                        new JsonSerializerSettings {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                }
            }
            catch (AdalServiceException ex)
            {
                ReportException(ex, msg);
            }
            catch (Exception ex)
            {
                ReportException(ex, msg);
            }
            ViewBag.Message = msg.ToString();

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ClientInfo()
        {
            ClientInfoViewModel model = new ClientInfoViewModel();

            StringBuilder msg = new StringBuilder();

            try
            {
                if (!Request.IsAuthenticated)
                    msg.AppendLine("Request not authenticated");
                var user = this.User;
                var claimsPrincipal = user as SSC.ClaimsPrincipal;
                if (claimsPrincipal != null)
                {
                    SSC.ClaimsIdentity claimsIdentity = claimsPrincipal.Identity as SSC.ClaimsIdentity;
                    if (claimsIdentity != null)
                    {
                        int claimNum = 1;
                        foreach (SSC.Claim claim in claimsIdentity.Claims)
                        {
                            model.ClientInfo.Add(new InfoItem
                            {
                                Name = string.Format("Claim {0}", claimNum),
                                Value = string.Format("Issuer: \"{1}\" {0}OriginalIssuer: \"{2}\" {0}Properties.Count: \"{3}\" {0}Subject: \"{4}\" {0}Type: \"{5}\" {0}ValueType: \"{6}\" {0}Value: \"{7}\" {0}",
                                        Environment.NewLine,
                                        claim.Issuer,
                                        claim.OriginalIssuer,
                                        claim.Properties.Count,
                                        claim.Subject,
                                        claim.Type,
                                        claim.ValueType,
                                        claim.Value)
                            });
                            claimNum++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportException(ex, msg);
            }
            model.Message = msg.ToString();

            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCDemo.ViewModels.Music;
using BGoodMusic.EFDAL.Interfaces;
using BGoodMusic.Models;
using MVCDemo.Infrastructure;
using MVCDemo.ViewModels.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MVCDemo.Controllers
{
    [RoutePrefix("demo/music")]
    [Route("{action}")]
    public class MusicController : DemoControllerBase
    {
        // GET: Music
        [Route]
        [Route("index")]
        public ActionResult Index()
        {
            IBGoodMusicRepository repo = new BGoodMusic.EFDAL.BGoodMusicDBContext();
            List<RehearsalListItem> itemList = new List<RehearsalListItem>();
            foreach(var r in repo.GetRehearsals().ToList())
            {
                itemList.Add(new RehearsalListItem
                    {
                        Id = r.Id,
                        Date = r.Date,
                        Duration = r.Duration,
                        Location = r.Location,
                        Time = r.Time
                    });
            }
            return View(itemList);
        }

        [Route("rehearsals")]
        public ActionResult Rehearsals()
        {
            StringBuilder msg = new StringBuilder();
            RehearsalViewModel model = new RehearsalViewModel();
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
                    TokenInfo ti = new TokenInfo
                    {
                        Tk = token
                    };
                    model.JsonToken = JsonConvert.SerializeObject(ti,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                }
            }
            catch (Exception ex)
            {
                ReportException(ex, msg);
            }
            ViewBag.Message = msg.ToString();
            return View(model);
        }
    }
}
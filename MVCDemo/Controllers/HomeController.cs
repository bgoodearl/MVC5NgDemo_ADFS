using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    [RoutePrefix("demo/home")]
    [Route("{action}")]
    public class HomeController : Controller
    {
        [Route("~/demo/")]
        [Route]
        [Route("index")]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}

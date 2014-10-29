using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    [RoutePrefix("demo/auth")]
    [Route("{action}")]
    public class AuthController : Controller
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
            var ctx = Request.GetOwinContext();
            ctx.Authentication.SignOut();
        }

    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthUtility;

namespace PortalServer.Controllers
{
    public class PlusController : PassAuthenticatedController
    {
        public ActionResult Login(string code)
        {
            PlusLogin(code);
            return Json(new { yourcode = code });
        }

        // DEBUG
        public ActionResult Modify()
        {
            /*var passCookie = Request.Cookies[PassCookieName];
            var pass = JsonConvert.DeserializeObject<Pass>(passCookie.Value);
            pass.data.userId = "I'm a bad guy! :))";
            Response.Cookies.Set(new HttpCookie(PassCookieName, JsonConvert.SerializeObject(pass)));*/
            Response.Cookies.Set(new HttpCookie(PassCookieName, "I'm a bad guy! :))"));
            return HttpNotFound();
        }

        // DEBUG
        public ActionResult GetProfile()
        {
            string clientId = ConfigurationManager.AppSettings["GoogleApiClientId"];
            string clientSecret = ConfigurationManager.AppSettings["GoogleApiClientSecret"];
            if (ValidPass == null)
                return HttpNotFound("Not authenticated");

            var ps = ValidPass.GetPlusService(clientId, clientSecret);

            return Json(new { profile = ps.People.Get("me").Fetch() });
        }
    }
}
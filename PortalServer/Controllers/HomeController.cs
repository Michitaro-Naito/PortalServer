using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalServer.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            /*string privateKeyXmlString;
            string publicKeyXmlString;
            using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
            {
                privateKeyXmlString = rsa.ToXmlString(true);
                publicKeyXmlString = rsa.ToXmlString(false);
                System.Diagnostics.Debug.WriteLine(privateKeyXmlString);
                System.Diagnostics.Debug.WriteLine(publicKeyXmlString);
            }*/
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult StoreToken()
        {
            return View();
        }
    }
}
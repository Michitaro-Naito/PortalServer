using AuthUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalServer.Controllers
{
    /// <summary>
    /// Authenticated by a signed Pass object.
    /// </summary>
    public class PassAuthenticatedController : BaseController
    {
        /// <summary>
        /// Cookie name of Pass object. User's browser remembers serialized Pass object for this name.
        /// </summary>
        protected const string PassCookieName = "Pass";

        /// <summary>
        /// Pass of User. If null, User is not authenticated.
        /// </summary>
        protected Pass ValidPass { get; private set; }
        protected string ValidPassString { get; private set; }

        void GetPass()
        {
            Pass pass = null;
            // Tries to retrieve Pass from Cookie.
            var passCookie = Request.Cookies[PassCookieName];
            string passString = null;
            if (passCookie != null)
            {
                passString = passCookie.Value;
                try
                {
                    //pass = JsonConvert.DeserializeObject<Pass>(passString);
                    pass = JsonConvert.DeserializeObject<Pass>(AuthHelper.Decrypt(passString));
                }
                catch
                {
                    pass = null;
                }
            }

            if (pass == null)
                // Not Authenticated
                return;

            /*if (!pass.IsValid)
            {
                // Invalid Pass. (Modified by Hacker?) Clears Cookie and return.
                //Debug.WriteLine("Invalid Pass. Discarding Cookie...");
                Response.Cookies.Remove(PassCookieName);
                return;
            }*/

            // Pass is valid. Sets it to Controller.
            ValidPass = pass;
            ValidPassString = passString;
        }

        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            GetPass();
            //Debug.WriteLine("Pass: " + ValidPass);
            base.OnAuthentication(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Debug.WriteLine("OnException");

            if (!filterContext.ExceptionHandled)
            {
                // Handle Exception...
                var type = filterContext.Exception.GetType();
                if (type == typeof(InvalidPassException))
                {
                    Debug.WriteLine("InvalidPassException");
                    filterContext.Result = View("InvalidPassException");
                    filterContext.ExceptionHandled = true;
                }
            }

            base.OnException(filterContext);
        }

        /// <summary>
        /// User gives Code (provided by Google+ OAuth2) to Server.
        /// Server gives Pass (signed User info, Cookie) to User.
        /// 
        /// ----- Pass -----
        /// Data:{
        ///     UserId: []  // Local User ID
        ///     Plus: {
        ///         // Google+ OAuth2, Authentication Object
        ///     }
        /// },
        /// Sign: []    // SHA512 Hash
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected void PlusLogin(string code)
        {
            // Manually perform the OAuth2 flow.
            string clientId = ConfigurationManager.AppSettings["GoogleApiClientId"];
            string clientSecret = ConfigurationManager.AppSettings["GoogleApiClientSecret"];
            var authObject = ManualCodeExchanger.ExchangeCode(clientId, clientSecret, code);

            // Generates Pass from AuthObject and stores it to Cookie.
            // (It describes User is who.)
            var pass = Pass.GenerateFromAuthObject(authObject);
            var json = JsonConvert.SerializeObject(pass);

            /*var key = Convert.FromBase64String("RYtoTxHdu9Er4F9oFdK3m7k8tpu9hitAxWPU9iFRubg=");
            var iv = Convert.FromBase64String("orfxwbQx7HoHja4SDWo9UA==");
            Debug.WriteLine(string.Format("Key:{0} IV:{1}", Convert.ToBase64String(key), Convert.ToBase64String(iv)));*/
            /*Debug.WriteLine("JSON: " + json);
            var encrypted = AuthHelper.Encrypt(json);
            Debug.WriteLine("Encrypted: " + encrypted);
            var decrypted = AuthHelper.Decrypt(encrypted);
            Debug.WriteLine("Decrypted: " + decrypted);
            if (decrypted == json)
                Debug.WriteLine("The same");*/


            //Response.Cookies.Set(new HttpCookie(PassCookieName, json));
            Response.Cookies.Set(new HttpCookie(PassCookieName, AuthHelper.Encrypt(json)));
        }
    }
}
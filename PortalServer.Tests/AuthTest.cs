using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthUtility;
using System.Configuration;

namespace PortalServer.Tests
{
    [TestClass]
    public class AuthTest
    {
        [TestMethod]
        public void GamePassSignTest()
        {
            var url = "http://amlitek.com/";
            var pass = new GamePass();
            pass.data.userId = "Foo";
            pass.data.redirectUrl = url;
            pass.data.expires = DateTime.UtcNow.AddHours(1);
            var privateKey = ConfigurationManager.AppSettings["PrivateKeyXmlString"];
            Assert.IsNotNull(privateKey);
            var publicKey = ConfigurationManager.AppSettings["PublicKeyXmlString"];
            Assert.IsNotNull(publicKey);
            pass.SignThis(privateKey);
            Assert.IsTrue(pass.IsValid(publicKey, url));
        }
    }
}

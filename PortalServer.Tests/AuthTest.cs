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
        public void EntryPassSignTest()
        {
            var uri = new Uri("http://amlitek.com/");
            var pass = new EntryPass();
            pass.data.userId = "Foo";
            pass.data.authority = uri.Authority;
            pass.data.expires = DateTime.UtcNow.AddHours(1);
            var privateKey = ConfigurationManager.AppSettings["PrivateKeyXmlString"];
            Assert.IsNotNull(privateKey);
            var publicKey = ConfigurationManager.AppSettings["PublicKeyXmlString"];
            Assert.IsNotNull(publicKey);
            pass.SignThis(privateKey);
            Assert.IsTrue(pass.IsValid(publicKey, uri.Authority));
        }
    }
}

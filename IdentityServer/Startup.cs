using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using IdentityServer.Helpers;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

[assembly: OwinStartup(typeof(IdentityServer.Startup))]

namespace IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.Map("/identity", identityServer =>
            {
                var identityServerFactory = new IdentityServerServiceFactory().Configure();
                identityServer.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "IdentityServer",
                    SigningCertificate = LoadCertificate(),
                    Factory = identityServerFactory,
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        EnableLocalLogin = false
                    }
                });
            });
        }

        private X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\{1}", AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["signing-certificate.name"]),
                (string)ConfigurationManager.AppSettings["signing-certificate.password"]);
        }
    }
}

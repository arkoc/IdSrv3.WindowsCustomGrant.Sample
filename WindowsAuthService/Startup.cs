using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer.WindowsAuthentication.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

[assembly: OwinStartup(typeof(WindowsAuthService.Startup))]

namespace WindowsAuthService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWindowsAuthenticationService(new WindowsAuthenticationOptions
            {
                IdpRealm = "urn:idsrv3",
                IdpReplyUrl = "https://localhost:44347/was",
                SigningCertificate = LoadCertificate(),
                EnableOAuth2Endpoint = true
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

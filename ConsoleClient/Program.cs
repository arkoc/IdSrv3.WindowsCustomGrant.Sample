using ConsoleApplication34;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Client;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            string IdentityServerRootUrl = "https://localhost:44347";
            string WindowsAuthenticationRootUrl = "https://localhost:44384/";

            "===============================================".ConsoleRed();
            "CONNECTING TO WINDOWS AUTH SERVER TO GET WIN TOKEN".ConsoleRed();
            "===============================================".ConsoleRed();

            Console.WriteLine(Environment.NewLine);

            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = true,
            };

            var oauthClientForToken = new OAuth2Client(
                new Uri($"{WindowsAuthenticationRootUrl}/token"),
                handler);

            var resultToken = oauthClientForToken.RequestCustomGrantAsync("windows").Result;

            ShowResponse(resultToken);

            Console.WriteLine(Environment.NewLine);

            "===============================================".ConsoleRed();
            "CONNECTING TO IDENTITY SERVER TO VALIDATE TOKEN".ConsoleRed();
            "===============================================".ConsoleRed();

            Console.WriteLine(Environment.NewLine);

            var clientForAuth = new OAuth2Client(
                new Uri($"{IdentityServerRootUrl}/identity/connect/token"));

            var additionalvalues = new Dictionary<string, string>()
            {
                { "client_id", "client" },
                { "client_secret", "secret" },
                { "win_token", resultToken.AccessToken }
            };

            var authResult = clientForAuth.RequestCustomGrantAsync("windows", "api", additionalvalues).Result;

            ShowResponse(authResult);

            Console.ReadLine();

        }

        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                "Token response:".ConsoleGreen();
                Console.WriteLine(response.Json);

                if (response.AccessToken.Contains("."))
                {
                    "\nAccess Token (decoded):".ConsoleGreen();

                    var parts = response.AccessToken.Split('.');
                    var header = parts[0];
                    var claims = parts[1];

                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header))));
                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims))));
                }
            }
            else
            {
                if (response.IsHttpError)
                {
                    "HTTP error: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorStatusCode);
                    "HTTP error reason: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorReason);
                }
                else
                {
                    "Protocol error response:".ConsoleGreen();
                    Console.WriteLine(response.Json);
                }
            }
        }
    }
}

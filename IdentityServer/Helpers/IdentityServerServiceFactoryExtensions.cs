using IdentityServer.Services;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityServer.Helpers
{
    public static class IdentityServerServiceFactoryExtensions
    {
        public static IdentityServerServiceFactory Configure(this IdentityServerServiceFactory factory)
        {
            factory.CustomGrantValidators.Add(new Registration<ICustomGrantValidator>(typeof(WindowsGrantValidator)));
            factory.UserService = new Registration<IUserService>(new ExternalRegistrationUserService());

            factory.UseInMemoryClients(Config.Clients);
            factory.UseInMemoryScopes(Config.Scopes);

            return factory;
        }
    }
}
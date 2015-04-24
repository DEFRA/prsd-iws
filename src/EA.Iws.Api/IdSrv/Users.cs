﻿namespace EA.Iws.Api.IdSrv
{
    using System.Collections.Generic;
    using Thinktecture.IdentityServer.Core.Services.InMemory;

    internal static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "bob",
                    Password = "secret",
                    Subject = "1"
                },
                new InMemoryUser
                {
                    Username = "alice",
                    Password = "secret",
                    Subject = "2"
                }
            };
        }
    }
}
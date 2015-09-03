namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using System.Security.Claims;
    using Prsd.Core.Domain;

    internal class TestUserContext : IUserContext
    {
        public TestUserContext(Guid returnsId)
        {
            ReturnsId = returnsId;
        }

        public Guid ReturnsId;

        public Guid UserId
        {
            get { return ReturnsId; }
        }

        public ClaimsPrincipal ReturnsPrincipal;

        public ClaimsPrincipal Principal
        {
            get { return ReturnsPrincipal; }
        }
    }
}
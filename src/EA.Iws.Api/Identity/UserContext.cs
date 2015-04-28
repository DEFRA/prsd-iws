namespace EA.Iws.Api.Identity
{
    using Core.Domain;
    using System;

    public class UserContext : IUserContext
    {
        public Guid UserId
        {
            get { throw new NotImplementedException(); }
        }
    }
}
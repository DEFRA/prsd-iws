namespace EA.Iws.Requests.Registration.Users
{
    using System;
    using Core.Registration.Users;
    using Prsd.Core.Mediator;

    public class UserById : IRequest<User>
    {
        public readonly string Id;

        public UserById(string id)
        {
            Id = id;
        }

        public UserById(Guid id)
        {
            Id = id.ToString();
        }
    }
}
namespace EA.Iws.Cqrs.Users
{
    using System;
    using Core.Cqrs;
    using Domain;

    public class UserById : IQuery<User>
    {
        public readonly string Id;

        public UserById(string id)
        {
            this.Id = id;
        }

        public UserById(Guid id)
        {
            this.Id = id.ToString();
        }
    }
}

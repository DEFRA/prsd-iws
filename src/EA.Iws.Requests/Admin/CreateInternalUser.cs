namespace EA.Iws.Requests.Admin
{
    using System;
    using Core.Notification;
    using Prsd.Core.Mediator;

    public class CreateInternalUser : IRequest<Guid>
    {
        public CreateInternalUser(string userId, string jobTitle, Guid localAreaId,
            CompetentAuthority competentAuthority)
        {
            UserId = userId;
            JobTitle = jobTitle;
            LocalAreaId = localAreaId;
            CompetentAuthority = competentAuthority;
        }

        public string UserId { get; set; }

        public string JobTitle { get; private set; }

        public Guid LocalAreaId { get; private set; }

        public CompetentAuthority CompetentAuthority { get; private set; }
    }
}
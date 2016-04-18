namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanCreateInternalUser)]
    public class CreateInternalUser : IRequest<Guid>
    {
        public CreateInternalUser(string userId, string jobTitle, Guid localAreaId,
            UKCompetentAuthority competentAuthority)
        {
            UserId = userId;
            JobTitle = jobTitle;
            LocalAreaId = localAreaId;
            CompetentAuthority = competentAuthority;
        }

        public string UserId { get; set; }

        public string JobTitle { get; private set; }

        public Guid LocalAreaId { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }
    }
}
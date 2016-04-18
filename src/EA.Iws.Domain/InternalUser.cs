namespace EA.Iws.Domain
{
    using System;
    using Core.Admin;
    using Core.Notification;
    using Events;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class InternalUser : Entity
    {
        protected InternalUser()
        {
        }

        public InternalUser(string userId, string jobTitle, UKCompetentAuthority competentAuthority, Guid localAreaId)
        {
            Guard.ArgumentNotNullOrEmpty(() => userId, userId);
            Guard.ArgumentNotNullOrEmpty(() => jobTitle, jobTitle);
            Guard.ArgumentNotDefaultValue(() => localAreaId, localAreaId);

            UserId = userId;
            JobTitle = jobTitle;
            CompetentAuthority = competentAuthority;
            LocalAreaId = localAreaId;
            Status = InternalUserStatus.Pending;
        }

        public string UserId { get; private set; }

        public virtual User User { get; protected set; }

        public string JobTitle { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public Guid LocalAreaId { get; private set; }

        public virtual LocalArea LocalArea { get; protected set; }

        public InternalUserStatus Status { get; private set; }

        public void Approve()
        {
            Status = InternalUserStatus.Approved;

            RaiseEvent(new RegistrationApprovedEvent(User.Email));
        }

        public void Reject()
        {
            Status = InternalUserStatus.Rejected;

            RaiseEvent(new RegistrationRejectedEvent(User.Email));
        }
    }
}
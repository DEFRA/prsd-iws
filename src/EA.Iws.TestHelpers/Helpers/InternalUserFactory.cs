namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Core.Notification;
    using Domain;

    public static class InternalUserFactory
    {
        public static InternalUser Create(Guid id, User user)
        {
            var internalUser = ObjectInstantiator<InternalUser>.CreateNew();

            ObjectInstantiator<InternalUser>.SetProperty(x => x.Id, id, internalUser);
            ObjectInstantiator<InternalUser>.SetProperty(x => x.User, user, internalUser);
            ObjectInstantiator<InternalUser>.SetProperty(x => x.UserId, user.Id, internalUser);

            ObjectInstantiator<InternalUser>.SetProperty(x => x.CompetentAuthority, UKCompetentAuthority.England, internalUser);

            return internalUser;
        }
    }
}
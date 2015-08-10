namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetUnitedKingdomCompetentAuthorityByNotificationId : IRequest<UnitedKingdomCompetentAuthorityData>
    {
        public Guid Id { get; private set; }

        public GetUnitedKingdomCompetentAuthorityByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}

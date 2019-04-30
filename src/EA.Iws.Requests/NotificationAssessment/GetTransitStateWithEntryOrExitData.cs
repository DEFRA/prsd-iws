namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeTransitStateEntryExitPoint)]
    public class GetTransitStateWithEntryOrExitData : IRequest<TransitStateWithEntryOrExitPointsData>
    {
        public Guid Id { get; private set; }

        public Guid TransitStateId { get; private set; }

        public GetTransitStateWithEntryOrExitData(Guid id, Guid transitStateId)
        {
            Id = id;
            TransitStateId = transitStateId;
        }
    }
}

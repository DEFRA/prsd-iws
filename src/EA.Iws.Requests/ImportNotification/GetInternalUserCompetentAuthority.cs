namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetInternalUserCompetentAuthority : IRequest<Tuple<CompetentAuthority, CompetentAuthorityData>>
    {
    }
}

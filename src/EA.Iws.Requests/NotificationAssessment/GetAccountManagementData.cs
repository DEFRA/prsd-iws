namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetAccountManagementData : IRequest<AccountManagementData>
    {
        public GetAccountManagementData(Guid id)
        {
            NotificationId = id;
        }

        public Guid NotificationId;
    }
}

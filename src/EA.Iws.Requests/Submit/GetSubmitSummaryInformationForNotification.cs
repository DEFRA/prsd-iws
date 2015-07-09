namespace EA.Iws.Requests.Submit
{
    using System;
    using Core.Notification;
    using Prsd.Core.Mediator;

    public class GetSubmitSummaryInformationForNotification : IRequest<SubmitSummaryData>
    {
        public Guid Id { get; private set; }

        public GetSubmitSummaryInformationForNotification(Guid id)
        {
            Id = id;
        }
    }
}

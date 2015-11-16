namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.ImportNotification.Summary;
    using Prsd.Core.Mediator;

    public class GetSummary : IRequest<InProgressImportNotificationSummary>
    {
        public Guid Id { get; set; }

        public GetSummary(Guid id)
        {
            Id = id;
        }
    }
}

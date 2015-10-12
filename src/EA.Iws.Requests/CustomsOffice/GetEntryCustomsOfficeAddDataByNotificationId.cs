namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.CustomsOffice;
    using Prsd.Core.Mediator;

    public class GetEntryCustomsOfficeAddDataByNotificationId : IRequest<EntryCustomsOfficeAddData>
    {
        public Guid Id { get; private set; }

        public GetEntryCustomsOfficeAddDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}

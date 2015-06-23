namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Prsd.Core.Mediator;

    public class GetExitCustomsOfficeAddDataByNotificationId : IRequest<ExitCustomsOfficeAddData>
    {
        public Guid Id { get; private set; }

        public GetExitCustomsOfficeAddDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}

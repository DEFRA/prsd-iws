namespace EA.Iws.Requests.StateOfImport
{
    using System;
    using Prsd.Core.Mediator;

    public class GetStateOfImportSetDataByNotificationId : IRequest<StateOfImportSetData>
    {
        public Guid Id { get; private set; }

        public GetStateOfImportSetDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}

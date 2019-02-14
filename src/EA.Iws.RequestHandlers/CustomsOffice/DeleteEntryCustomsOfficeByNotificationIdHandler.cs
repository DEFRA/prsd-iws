namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.CustomsOffice;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class DeleteEntryCustomsOfficeByNotificationIdHandler :
        IRequestHandler<DeleteEntryCustomsOfficeByNotificationId, bool>
    {
        private readonly ITransportRouteRepository repository;

        public DeleteEntryCustomsOfficeByNotificationIdHandler(ITransportRouteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteEntryCustomsOfficeByNotificationId message)
        {
            await repository.DeleteEntryCustomsOfficeByNotificationId(message.NotificationId);

            return true;
        }
    }
}

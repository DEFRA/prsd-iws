namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.OperationCodes;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class AddDisposalCodesHandler : IRequestHandler<AddDisposalCodes, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<IList<DisposalCode>, IList<OperationCode>> mapper;

        public AddDisposalCodesHandler(IwsContext db, IMap<IList<DisposalCode>, IList<OperationCode>> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(AddDisposalCodes command)
        {
            var disposalCodes = mapper.Map(command.DisposalCodes);

            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            notification.SetOperationCodes(disposalCodes);

            await db.SaveChangesAsync();

            return command.NotificationId;
        }
    }
}
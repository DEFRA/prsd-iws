namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.OperationCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class AddDisposalCodesHandler : IRequestHandler<AddDisposalCodes, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<IList<DisposalCode>, IList<OperationCode>> mapper;

        public AddDisposalCodesHandler(IwsContext context, IMap<IList<DisposalCode>, IList<OperationCode>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(AddDisposalCodes command)
        {
            var disposalCodes = mapper.Map(command.DisposalCodes);

            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetOperationCodes(disposalCodes);

            await context.SaveChangesAsync();

            return command.NotificationId;
        }
    }
}
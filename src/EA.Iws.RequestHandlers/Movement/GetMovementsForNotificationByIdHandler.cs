namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementsForNotificationByIdHandler : IRequestHandler<GetMovementsForNotificationById, Dictionary<int, Guid>>
    {
        private readonly IwsContext context;

        public GetMovementsForNotificationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Dictionary<int, Guid>> HandleAsync(GetMovementsForNotificationById message)
        {
            return await context.Movements
                .Where(m => message.NotificationId == m.NotificationId)
                .ToDictionaryAsync(m => m.Number, m => m.Id);
        }
    }
}

namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    public class GetMovementsForNotificationByIdHandler : IRequestHandler<GetMovementsForNotificationById, Dictionary<int, Guid>>
    {
        private readonly IwsContext context;

        public GetMovementsForNotificationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Dictionary<int, Guid>> HandleAsync(GetMovementsForNotificationById message)
        {
            return await context.Movements.ToDictionaryAsync(m => m.Number, m => m.Id);
        }
    }
}

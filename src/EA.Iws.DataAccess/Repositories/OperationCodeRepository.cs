namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Core.Shared;
    using EA.Iws.Domain.OperationCodes;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class OperationCodeRepository : IOperationCodeRepository
    {
        private readonly IwsContext context;

        public OperationCodeRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OperationCode>> GetOperationCodes(NotificationType notificationType, bool isInterim)
        {
            if (isInterim)
            {
                return await context.OperationCodes
                                    .Where(p => p.NotificationType == notificationType && p.IsInterim == isInterim)
                                    .OrderBy(p => p.IsInterim)
                                    .OrderByDescending(p => p.Id)
                                    .ToArrayAsync();
            }
            else
            {
                return await context.OperationCodes
                                    .Where(p => p.NotificationType == notificationType && p.IsInterim == isInterim)
                                    .OrderBy(p => p.Id)
                                    .ToArrayAsync();
            }
        }
    }
}

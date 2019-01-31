namespace EA.Iws.DataAccess.Draft
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Domain.Movement.BulkUpload;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class DraftMovementRepository : IDraftMovementRepository
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public DraftMovementRepository(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Guid> Add(Guid notificationId, List<PrenotificationMovement> movements, string fileName)
        {
            Guid result;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var draftMovement = new DraftBulkUpload()
                    {
                        NotificationId = notificationId,
                        CreatedDate = SystemTime.UtcNow,
                        CreatedBy = userContext.UserId.ToString(),
                        FileName = fileName
                    };

                    context.DraftBulkUploads.Add(draftMovement);

                    await context.SaveChangesAsync();

                    foreach (var movement in movements)
                    {
                        var draftPrenotifications = new DraftMovement(draftMovement.Id, movement);

                        context.DraftMovements.Add(draftPrenotifications);

                        await context.SaveChangesAsync();
                    }

                    result = draftMovement.Id;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }

                transaction.Commit();
            }

            return result;
        }
    }
}

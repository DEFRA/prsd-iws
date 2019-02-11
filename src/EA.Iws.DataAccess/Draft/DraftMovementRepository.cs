namespace EA.Iws.DataAccess.Draft
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Shared;
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

        public async Task<Guid> AddPrenotifications(Guid notificationId, List<PrenotificationMovement> movements, string fileName)
        {
            Guid result;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var draftBulkUpload = new DraftBulkUpload(notificationId, SystemTime.UtcNow,
                        userContext.UserId.ToString(), fileName);

                    context.DraftBulkUploads.Add(draftBulkUpload);

                    await context.SaveChangesAsync();

                    var draftMovements = await GetDraftMovements(draftBulkUpload.Id, movements);

                    foreach (var draftMovement in draftMovements)
                    {
                        context.DraftMovements.Add(draftMovement);

                        await context.SaveChangesAsync();
                    }

                    result = draftBulkUpload.Id;
                }
                catch
                { 
                    transaction.Rollback();
                    throw;
                }

                transaction.Commit();
            }

            return result;
        }

        public async Task<IEnumerable<DraftMovement>> GetDraftMovementById(Guid draftBulkUploadId)
        {
            return await context.DraftMovements.Where(m => m.BulkUploadId == draftBulkUploadId).ToArrayAsync();
        }

        public async Task<bool> DeleteDraftMovementByNotificationId(Guid notificationId)
        {
            var rowsAffected = await context.Database.ExecuteSqlCommandAsync(@"
                DELETE DPI FROM [Draft].[PackagingInfo] DPI JOIN [Draft].[Movement] DM ON DPI.DraftMovementId = DM.Id JOIN [Draft].[BulkUpload] DBU ON DM.BulkUploadId = DBU.Id WHERE DBU.NotificationId = @Id
                DELETE DM FROM [Draft].[Movement] DM JOIN [Draft].[BulkUpload] DBU ON DM.BulkUploadId = DBU.Id WHERE DBU.NotificationId = @Id
                DELETE FROM [Draft].[BulkUpload] WHERE NotificationId = @Id",
                new SqlParameter("@Id", notificationId));

            return rowsAffected > 0;
        }

        private static async Task<IEnumerable<DraftMovement>> GetDraftMovements(Guid draftMovementId, List<PrenotificationMovement> movements)
        {
            return await Task.Run(() =>
            {
                return movements.Select(movement =>
                {
                    var shipmentNumber = movement.ShipmentNumber.HasValue ? movement.ShipmentNumber.Value : 0;
                    var quantity = movement.Quantity.HasValue ? movement.Quantity.Value : 0m;
                    var units = movement.Unit.HasValue ? movement.Unit.Value : default(ShipmentQuantityUnits);
                    var shipmentDate = movement.ActualDateOfShipment.HasValue
                        ? movement.ActualDateOfShipment.Value
                        : default(DateTime);
                    var packagingInfos = movement.PackagingTypes.Select(p => new DraftPackagingInfo() { PackagingType = p }).ToList();

                    return new DraftMovement(draftMovementId, movement.NotificationNumber,
                            shipmentNumber, quantity, units,
                            shipmentDate, packagingInfos);
                });
            });
        }
    }
}

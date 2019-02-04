namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationContentExcessiveShipmentsRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationMovementsSummaryRepository repo;

       public PrenotificationContentExcessiveShipmentsRule(INotificationMovementsSummaryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var movementSummary = await repo.GetById(notificationId);

            return await Task.Run(() =>
            {
                var remainingShipments = movementSummary.ActiveLoadsPermitted - movementSummary.CurrentActiveLoads;

                var result = remainingShipments < movements.Count ? MessageLevel.Error : MessageLevel.Success;
                
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.ExcessiveShipments), movements.Count, remainingShipments);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.ExcessiveShipments, result, errorMessage);
            });
        }
    }
}
namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationExcessiveShipmentsRule : IPrenotificationContentRule
    {
        private readonly INotificationMovementsSummaryRepository repo;

       public PrenotificationExcessiveShipmentsRule(INotificationMovementsSummaryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var movementSummary = await repo.GetById(notificationId);

            return await Task.Run(() =>
            {
                var remainingShipments = movementSummary.ActiveLoadsPermitted - movementSummary.CurrentActiveLoads;

                var result = remainingShipments < movements.Count ? MessageLevel.Error : MessageLevel.Success;
                
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.ExcessiveShipments), movements.Count, remainingShipments);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.ExcessiveShipments, result, errorMessage);
            });
        }
    }
}
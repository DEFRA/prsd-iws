namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Rules;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using EA.Iws.Core.Movement.Bulk;

    public class PrenotificationExcessiveShipmentsDataRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationMovementsSummaryRepository notificationMovementsSummaryRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationExcessiveShipmentsDataRule(INotificationMovementsSummaryRepository notificationMovementsSummaryRepository,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationMovementsSummaryRepository = notificationMovementsSummaryRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            var excessiveShipmentsResult = MessageLevel.Success;
            var excessiveShipmentNumbers = new List<string>();
            string errorMessage = string.Empty;

            var movementSummary = await notificationMovementsSummaryRepository.GetById(notificationId);

            int remainingShipments = 1; //movementSummary.ActiveLoadsPermitted - movementSummary.CurrentActiveLoads;

            if (remainingShipments < shipments.Count)
            {
                excessiveShipmentsResult = MessageLevel.Error;
            }

            return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.ExcessiveShipments, excessiveShipmentsResult, excessiveShipmentNumbers, remainingShipments, shipments.Count);
        }
    }
}

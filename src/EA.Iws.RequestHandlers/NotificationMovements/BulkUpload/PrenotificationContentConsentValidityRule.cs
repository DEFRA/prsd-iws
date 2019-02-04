namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationConsent;

    public class PrenotificationContentConsentValidityRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationConsentRepository notificationConsentRepository;

        public PrenotificationContentConsentValidityRule(INotificationConsentRepository notificationConsentRepository)
        {
            this.notificationConsentRepository = notificationConsentRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var consent = await notificationConsentRepository.GetByNotificationId(notificationId);

            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                (m.ActualDateOfShipment.HasValue &&
                                 !consent.ConsentRange.Contains(m.ActualDateOfShipment.Value)))
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                
                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.ConsentValidity), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.ConsentValidity, result, errorMessage);
            });
        }
    }
}

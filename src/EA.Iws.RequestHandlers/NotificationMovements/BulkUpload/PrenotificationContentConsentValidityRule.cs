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
                var contentValidityResult = MessageLevel.Success;
                var contentValidityShipmentNumbers = new List<string>();

                foreach (var shipment in movements)
                {
                    if (shipment.ShipmentNumber.HasValue && shipment.ActualDateOfShipment.HasValue)
                    {
                        if (!consent.ConsentRange.Contains(shipment.ActualDateOfShipment.GetValueOrDefault()))
                        {
                            contentValidityResult = MessageLevel.Error;
                            contentValidityShipmentNumbers.Add(shipment.ShipmentNumber.ToString());
                        }
                    }
                }
                
                var shipmentNumbers = string.Join(", ", contentValidityShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.ConsentValidity), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.ConsentValidity, contentValidityResult, errorMessage);
            });
        }
    }
}

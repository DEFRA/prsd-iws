namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.NotificationConsent;

    public class PrenotificationContentShipmentBeyondConsentedDateRule : IPrenotificationContentRule
    {
        private readonly INotificationConsentRepository consentRepository;

        public PrenotificationContentShipmentBeyondConsentedDateRule(INotificationConsentRepository consentRepository)
        {
            this.consentRepository = consentRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var consentEndDate = (await consentRepository.GetByNotificationId(notificationId)).ConsentRange.To;

            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue && m.ActualDateOfShipment.HasValue &&
                                m.ActualDateOfShipment.Value > consentEndDate)
                                .OrderBy(m => m.ActualDateOfShipment.Value)
                        .Select(m => m.ShipmentNumber.GetValueOrDefault())
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.BeyondConsentWindow), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.BeyondConsentWindow, result, errorMessage);
            });
        }
    }
}
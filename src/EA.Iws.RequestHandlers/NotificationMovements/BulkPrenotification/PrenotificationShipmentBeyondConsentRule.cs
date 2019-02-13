namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.NotificationConsent;

    public class PrenotificationShipmentBeyondConsentRule : IPrenotificationContentRule
    {
        private readonly INotificationConsentRepository consentRepository;

        public PrenotificationShipmentBeyondConsentRule(INotificationConsentRepository consentRepository)
        {
            this.consentRepository = consentRepository;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
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
                        .GroupBy(x => x.ShipmentNumber)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.BeyondConsentWindow), shipmentNumbers);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.BeyondConsentWindow, result, errorMessage);
            });
        }
    }
}
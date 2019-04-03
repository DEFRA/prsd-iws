namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.NotificationConsent;

    public class PrenotificationConsentValidityRule : IPrenotificationContentRule
    {
        private readonly INotificationConsentRepository notificationConsentRepository;

        public PrenotificationConsentValidityRule(INotificationConsentRepository notificationConsentRepository)
        {
            this.notificationConsentRepository = notificationConsentRepository;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
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
                        .GroupBy(m => m.ShipmentNumber)
                        .OrderBy(x => x.Key)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var minShipment = shipments.FirstOrDefault() ?? 0;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.ConsentValidity), shipmentNumbers);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.ConsentValidity, result, errorMessage, minShipment);
            });
        }
    }
}

namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationConsent;
    using Prsd.Core;

    public class PrenotificationContentThreeWorkingDaysConsentDateRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationConsentRepository consentRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationContentThreeWorkingDaysConsentDateRule(INotificationConsentRepository consentRepository,
            IWorkingDayCalculator workingDayCalculator,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.consentRepository = consentRepository;
            this.workingDayCalculator = workingDayCalculator;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements,
            Guid notificationId)
        {
            var ca = (await notificationApplicationRepository.GetById(notificationId)).CompetentAuthority;
            var consentEndDate = (await consentRepository.GetByNotificationId(notificationId)).ConsentRange.To;
            var consentHasExpired = consentEndDate < SystemTime.UtcNow;

            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue && m.ActualDateOfShipment.HasValue &&
                                m.ActualDateOfShipment.Value > SystemTime.UtcNow && !consentHasExpired &&
                                workingDayCalculator.GetWorkingDays(m.ActualDateOfShipment.Value, consentEndDate, true, ca) < 4)
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.ThreeWorkingDaysToConsentDate), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.ThreeWorkingDaysToConsentDate, result, errorMessage);
            });
        }
    }
}

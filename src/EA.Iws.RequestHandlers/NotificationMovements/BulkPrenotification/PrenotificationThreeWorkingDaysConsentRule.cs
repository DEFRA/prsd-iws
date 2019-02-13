namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationConsent;
    using Prsd.Core;

    public class PrenotificationThreeWorkingDaysConsentRule : IPrenotificationContentRule
    {
        private readonly INotificationConsentRepository consentRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationThreeWorkingDaysConsentRule(INotificationConsentRepository consentRepository,
            IWorkingDayCalculator workingDayCalculator,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.consentRepository = consentRepository;
            this.workingDayCalculator = workingDayCalculator;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(
            List<PrenotificationMovement> movements,
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
                                m.ActualDateOfShipment.Value.Date >= SystemTime.UtcNow.Date && !consentHasExpired &&
                                workingDayCalculator.GetWorkingDays(m.ActualDateOfShipment.Value, consentEndDate, true,
                                    ca) < 4)
                        .GroupBy(x => x.ShipmentNumber)
                        .OrderBy(x => x.Key)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var minShipment = shipments.FirstOrDefault() ?? 0;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(
                            PrenotificationContentRules.ThreeWorkingDaysToConsentDate), shipmentNumbers);

                return
                    new PrenotificationContentRuleResult<PrenotificationContentRules>(
                        PrenotificationContentRules.ThreeWorkingDaysToConsentDate, result, errorMessage, minShipment);
            });
        }
    }
}

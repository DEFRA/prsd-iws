namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
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
                var result = MessageLevel.Success;
                var failedShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    if (movement.ShipmentNumber.HasValue && movement.ActualDateOfShipment.HasValue && !consentHasExpired)
                    {
                        var workingDays = workingDayCalculator.GetWorkingDays(movement.ActualDateOfShipment.Value,
                            consentEndDate, true, ca);

                        if (workingDays < 4)
                        {
                            result = MessageLevel.Error;
                            failedShipmentNumbers.Add(movement.ShipmentNumber.Value.ToString());
                        }
                    }
                }

                var shipmentNumbers = string.Join(", ", failedShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.ThreeWorkingDaysToConsentDate), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.ThreeWorkingDaysToConsentDate, result, errorMessage);
            });
        }
    }
}

namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationApplication;

    public class PrenotificationContentInvalidPackagingTypeRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationContentInvalidPackagingTypeRule(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var notificationApplication = await notificationApplicationRepository.GetById(notificationId);

            return await Task.Run(() =>
            {
                var notificationNumberResult = MessageLevel.Success;
                var notificationNumberShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    foreach (var packageType in movement.PackagingTypes)
                    {
                        if (notificationApplication.PackagingInfos.Count(p => p.PackagingType == packageType) == 0)
                        {
                            notificationNumberShipmentNumbers.Add(movement.ShipmentNumber.ToString());
                            break;
                        }
                    }
                }
                if (notificationNumberShipmentNumbers.Count > 0)
                {
                    notificationNumberResult = MessageLevel.Error;
                }

                var shipmentNumbers = string.Join(", ", notificationNumberShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.InvalidPackagingType), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.InvalidPackagingType, notificationNumberResult, errorMessage);
            });
        }
    }
}

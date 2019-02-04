namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                (!m.PackagingTypes.Any() ||
                                m.PackagingTypes.All(
                                    p => notificationApplication.PackagingInfos.All(t => t.PackagingType != p))))
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.InvalidPackagingType), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.InvalidPackagingType, result, errorMessage);
            });
        }
    }
}

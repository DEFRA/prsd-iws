namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SaveWasteRecovery : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public decimal PercentageRecoverable { get; private set; }
        public ValuePerWeightData EstimatedValue { get; private set; }
        public ValuePerWeightData RecoveryCost { get; private set; }

        public SaveWasteRecovery(Guid notificationId, 
            decimal percentageRecoverable, 
            ValuePerWeightData estimatedValue, 
            ValuePerWeightData recoveryCost)
        {
            RecoveryCost = recoveryCost;
            EstimatedValue = estimatedValue;
            PercentageRecoverable = percentageRecoverable;
            NotificationId = notificationId;
        }
    }
}

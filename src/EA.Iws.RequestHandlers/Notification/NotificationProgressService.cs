namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using NotificationType = Core.Shared.NotificationType;

    internal class NotificationProgressService : INotificationProgressService
    {
        private readonly IwsContext context;

        public NotificationProgressService(IwsContext context)
        {
            this.context = context;
        }

        public bool IsComplete(Guid notificationId)
        {
            var progress = GetNotificationProgressInfo(notificationId);

            return progress.IsAllComplete;
        }

        public NotificationApplicationCompletionProgress GetNotificationProgressInfo(Guid notificationId)
        {
            var progress = context.Database.SqlQuery<NotificationApplicationCompletionProgress>(
                "[Notification].[uspNotificationProgress] @NotificationId", new SqlParameter("@NotificationId", notificationId)).Single();

            progress.IsAllComplete = progress.HasOperationCodes && progress.HasPhysicalCharacteristics &&
                                     progress.HasWasteCodes
                                     && progress.HasCarrier && progress.HasWasteType
                                     && progress.HasExporter && progress.HasFacility && progress.HasImporter &&
                                     progress.HasShipmentInfo
                                     && progress.HasMeansOfTransport && progress.HasPackagingInfo &&
                                     progress.HasPreconsentedInformation &&
                                     progress.HasWasteGenerationProcess
                                     && progress.HasProducer && progress.HasReasonForExport &&
                                     progress.HasSpecialHandlingRequirements &&
                                     progress.HasStateOfExport
                                     && progress.HasStateOfImport && progress.HasTechnologyEmployed
                                     && progress.HasSiteOfExport && progress.HasActualSiteOfTreatment
                                     &&
                                     (progress.NotificationType == NotificationType.Disposal ||
                                      (progress.HasRecoveryInfo
                                       && progress.HasRecoveryData));

            return progress;
        }
    }
}
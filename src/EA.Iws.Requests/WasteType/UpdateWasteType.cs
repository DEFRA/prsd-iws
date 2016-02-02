namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteType;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class UpdateWasteType : IRequest<Guid>
    {
        public UpdateWasteType(Guid notificationId, ChemicalComposition chemicalCompositionType,
            string furtherInformation, List<WasteTypeCompositionData> wasteCompositions)
        {
            NotificationId = notificationId;
            ChemicalCompositionType = chemicalCompositionType;
            ChemicalCompositionDescription = FurtherInformation;
            WasteCompositions = wasteCompositions;
            FurtherInformation = furtherInformation;
        }

        public Guid NotificationId { get; private set; }

        public ChemicalComposition ChemicalCompositionType { get; private set; }

        public string ChemicalCompositionDescription { get; private set; }

        public string FurtherInformation { get; private set; }

        public List<WasteTypeCompositionData> WasteCompositions { get; private set; }
    }
}
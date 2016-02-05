namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class CreateWasteType : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public ChemicalComposition ChemicalCompositionType { get; set; }

        public string ChemicalCompositionDescription { get; set; }

        public List<WoodInformationData> WasteCompositions { get; set; }

        public string WasteCompositionName { get; set; }
    }
}
namespace EA.Iws.Requests.ImportNotification.WasteType
{
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetAllWasteCodes : IRequest<IList<WasteCodeData>>
    {
    }
}
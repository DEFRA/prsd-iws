namespace EA.Iws.Requests.WasteCodes
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetWasteCodesByType : IRequest<WasteCodeData[]>
    {
        public GetWasteCodesByType(params CodeType[] codeTypeses)
        {
            CodeTypes = codeTypeses;
        }

        public CodeType[] CodeTypes { get; private set; }
    }
}
namespace EA.Iws.Requests.Annexes
{
    using Core.Annexes;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetTechnologyEmployedAnnex : IRequest<bool>
    {
        public AnnexUpload Annex { get; private set; }

        public SetTechnologyEmployedAnnex(AnnexUpload annex)
        {
            Annex = annex;
        }
    }
}

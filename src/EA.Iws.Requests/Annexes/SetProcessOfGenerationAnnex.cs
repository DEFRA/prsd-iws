namespace EA.Iws.Requests.Annexes
{
    using Core.Annexes;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetProcessOfGenerationAnnex : IRequest<bool>
    {
        public AnnexUpload Annex { get; private set; }

        public SetProcessOfGenerationAnnex(AnnexUpload annex)
        {
            Annex = annex;
        }
    }
}

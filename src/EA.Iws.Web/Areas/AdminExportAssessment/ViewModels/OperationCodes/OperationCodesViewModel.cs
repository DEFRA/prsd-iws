namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.OperationCodes
{
    using EA.Iws.Core.Shared;
    using Web.Areas.AdminExportAssessment.Views.OperationCodes;

    public class OperationCodesViewModel
    {
        public string Title
        {
            get
            {
                return EditResources.Title.Replace("{type}", this.NotificationType.ToString());
            }
        }
        public string Header
        {
            get
            {
                return EditResources.Header.Replace("{type}", this.NotificationType.ToString());
            }
        }

        public NotificationType NotificationType { get; set; }

        public OperationCodesViewModel()
        {
            this.NotificationType = NotificationType.Recovery;
        }
    }
}
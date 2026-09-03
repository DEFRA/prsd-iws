namespace EA.Iws.Web.Areas.Admin.ViewModels.Menu
{
    using Core.ImportNotification;
    using Infrastructure;

    public class ImportNavigationViewModel : ReportLinkViewModel
    {
        public ImportNavigationSection ActiveSection { get; set; }

        public NotificationDetails Details { get; set; }

        public AdminLinksViewModel AdminLinksModel { get; set; }

        public bool ShowImportSections { get; set; }

        public bool ShowAssessmentDecision { get; set; }

        public bool ShowKeyDatesOverride { get; set; }

        public bool HasComments { get; set; }

        public bool ShowConsentExpiryDateInRed { get; set; }

        public System.DateTime? ConsentExpiryDate { get; set; }

        public System.DateTime? ConsentStartDate { get; set; }

        public System.DateTime? ConsentedDate { get; set; }
    }
}
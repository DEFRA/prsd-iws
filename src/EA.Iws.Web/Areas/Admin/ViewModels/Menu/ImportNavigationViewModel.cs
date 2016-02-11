namespace EA.Iws.Web.Areas.Admin.ViewModels.Menu
{
    using Core.ImportNotification;
    using Infrastructure;

    public class ImportNavigationViewModel
    {
        public ImportNavigationSection ActiveSection { get; set; }

        public NotificationDetails Details { get; set; }

        public AdminLinksViewModel AdminLinksModel { get; set; }

        public bool ShowImportSections { get; set; }
    }
}
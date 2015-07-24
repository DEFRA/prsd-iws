namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;

    internal class TransportViewModel
    {
        public TransportViewModel(NotificationApplication notification)
        {
            ExportCountry = notification.StateOfExport.Country.Name;
            ExportCompetentAuthority = notification.StateOfExport.CompetentAuthority.Code;
            ExitPoint = notification.StateOfExport.ExitPoint.Name;

            ImportCountry = notification.StateOfImport.Country.Name;
            ImportCompetentAuthority = notification.StateOfImport.CompetentAuthority.Code;
            EntryPoint = notification.StateOfImport.EntryPoint.Name;
        }

        public string ExportCountry { get; private set; }
        public string ExportCompetentAuthority { get; private set; }
        public string ExitPoint { get; private set; }

        public string ImportCountry { get; private set; }
        public string ImportCompetentAuthority { get; private set; }
        public string EntryPoint { get; private set; }
    }
}
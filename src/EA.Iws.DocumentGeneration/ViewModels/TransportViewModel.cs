namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.TransportRoute;

    internal class TransportViewModel
    {
        public TransportViewModel(TransportRoute transportRoute)
        {
            ExportCountry = transportRoute.StateOfExport.Country.Name;
            ExportCompetentAuthority = transportRoute.StateOfExport.CompetentAuthority.Code;
            ExitPoint = transportRoute.StateOfExport.ExitPoint.Name;

            ImportCountry = transportRoute.StateOfImport.Country.Name;
            ImportCompetentAuthority = transportRoute.StateOfImport.CompetentAuthority.Code;
            EntryPoint = transportRoute.StateOfImport.EntryPoint.Name;
        }

        public string ExportCountry { get; private set; }

        public string ExportCompetentAuthority { get; private set; }

        public string ExitPoint { get; private set; }

        public string ImportCountry { get; private set; }

        public string ImportCompetentAuthority { get; private set; }

        public string EntryPoint { get; private set; }
    }
}
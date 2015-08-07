namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.CustomsOffice;
    using Core.Notification;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Requests.Notification;

    public class JourneyViewModel
    {
        public Guid NotificationId { get; set; }
        public bool IsStateOfExportCompleted { get; set; }
        public bool IsStateOfImportCompleted { get; set; }
        public bool AreTransitStatesCompleted { get; set; }
        public bool IsCustomsOfficeCompleted { get; set; }
        public StateOfExportData StateOfExportData { get; set; }
        public List<TransitStateData> TransitStates { get; set; }
        public StateOfImportData StateOfImportData { get; set; }
        public CustomsOfficeData EntryCustomsOffice { get; set; }
        public CustomsOfficeData ExitCustomsOffice { get; set; }

        public JourneyViewModel()
        {
        }

        public JourneyViewModel(JourneyInfo journeyInfo)
        {
            NotificationId = journeyInfo.NotificationId;
            IsStateOfExportCompleted = journeyInfo.IsStateOfExportCompleted;
            IsStateOfImportCompleted = journeyInfo.IsStateOfImportCompleted;
            AreTransitStatesCompleted = journeyInfo.AreTransitStatesCompleted;
            IsCustomsOfficeCompleted = journeyInfo.IsCustomsOfficeCompleted;
            StateOfExportData = journeyInfo.TransportRoute.StateOfExport;
            TransitStates = journeyInfo.TransportRoute.TransitStates.ToList();
            StateOfImportData = journeyInfo.TransportRoute.StateOfImport;
            EntryCustomsOffice = journeyInfo.EntryCustomsOffice.CustomsOfficeData ?? new CustomsOfficeData();
            ExitCustomsOffice = journeyInfo.ExitCustomsOffice.CustomsOfficeData ?? new CustomsOfficeData();
        }
    }
}
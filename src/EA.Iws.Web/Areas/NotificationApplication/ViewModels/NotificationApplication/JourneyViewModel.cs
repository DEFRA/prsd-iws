namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.CustomsOffice;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;

    public class JourneyViewModel
    {
        public JourneyViewModel()
        {
        }

        public JourneyViewModel(Journey journeyInfo, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = journeyInfo.NotificationId;
            IsStateOfExportCompleted = progress.HasStateOfExport;
            IsStateOfImportCompleted = progress.HasStateOfImport;
            AreTransitStatesCompleted = progress.HasTransitState;
            IsCustomsOfficeCompleted = progress.HasCustomsOffice;
            StateOfExportData = journeyInfo.TransportRoute.StateOfExport;
            TransitStates = journeyInfo.TransportRoute.TransitStates.ToList();
            StateOfImportData = journeyInfo.TransportRoute.StateOfImport;
            EntryCustomsOffice = journeyInfo.EntryCustomsOffice.CustomsOfficeData ?? new CustomsOfficeData();
            ExitCustomsOffice = journeyInfo.ExitCustomsOffice.CustomsOfficeData ?? new CustomsOfficeData();
        }

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

        public bool CanChangeEntryExitPoint { get; set; }

        public bool CanAddRemoveTransitState { get; set; }
    }
}
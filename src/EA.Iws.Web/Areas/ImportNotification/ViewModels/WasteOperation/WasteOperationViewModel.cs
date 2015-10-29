namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteOperation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification;
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class WasteOperationViewModel
    {
        public WasteOperationViewModel()
        {
        }

        public WasteOperationViewModel(NotificationDetails details)
        {
            ImportNotificationId = details.ImportNotificationId;
            NotificationType = details.NotificationType;
        }

        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public CheckBoxCollectionViewModel Codes { get; set; }

        public string TechnologyEmployed { get; set; }

        [Display(Name = "TechnologyEmployedUploadedLater", ResourceType = typeof(WasteOperationViewModelResources))]
        public bool TechnologyEmployedUploadedLater { get; set; }
    }
}
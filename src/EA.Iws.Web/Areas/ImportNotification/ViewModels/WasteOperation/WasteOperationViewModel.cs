namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteOperation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.ImportNotification;
    using Core.ImportNotification.Draft;
    using Core.OperationCodes;
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class WasteOperationViewModel
    {
        public WasteOperationViewModel()
        {
        }

        public WasteOperationViewModel(NotificationDetails details, WasteOperation data)
        {
            ImportNotificationId = details.ImportNotificationId;
            NotificationType = details.NotificationType;

            if (details.NotificationType == NotificationType.Recovery)
            {
                Codes = CheckBoxCollectionViewModel.CreateFromEnum<RecoveryCode>();
            }
            else
            {
                Codes = CheckBoxCollectionViewModel.CreateFromEnum<DisposalCode>();
            }

            if (data.OperationCodes != null)
            {
                Codes.SetSelectedValues(data.OperationCodes);
            }

            TechnologyEmployed = data.TechnologyEmployed;
            TechnologyEmployedUploadedLater = data.TechnologyEmployedUploadedLater;
        }

        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public CheckBoxCollectionViewModel Codes { get; set; }

        public string TechnologyEmployed { get; set; }

        [Display(Name = "TechnologyEmployedUploadedLater", ResourceType = typeof(WasteOperationViewModelResources))]
        public bool TechnologyEmployedUploadedLater { get; set; }

        public int[] SelectedCodes
        {
            get { return Codes.PossibleValues.Where(p => p.Selected).Select(p => int.Parse(p.Value)).ToArray(); }
        }
    }
}
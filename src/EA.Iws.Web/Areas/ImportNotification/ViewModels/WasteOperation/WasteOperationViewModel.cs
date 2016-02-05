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
                //Codes = CheckBoxCollectionViewModel.CreateFromEnum<RecoveryCode>();
            }
            else
            {
                //Codes = CheckBoxCollectionViewModel.CreateFromEnum<DisposalCode>();
            }

            if (data.OperationCodes != null)
            {
                Codes.SetSelectedValues(data.OperationCodes);
            }

            TechnologyEmployed = data.TechnologyEmployed;
        }

        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public CheckBoxCollectionViewModel Codes { get; set; }

        [Display(Name = "TechnologyEmployed", ResourceType = typeof(WasteOperationViewModelResources))]
        public string TechnologyEmployed { get; set; }

        public OperationCode[] SelectedCodes
        {
            get { return Codes.PossibleValues.Where(p => p.Selected).Select(p => (OperationCode)Enum.Parse(typeof(OperationCode), p.Value)).ToArray(); }
        }
    }
}
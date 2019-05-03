namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.ImportNotification.Update;
    using Core.OperationCodes;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class UpdateWasteOperationViewModel
    {
        public UpdateWasteOperationViewModel()
        {
        }

        public UpdateWasteOperationViewModel(WasteOperationData data)
        {
            ImportNotificationId = data.Details.ImportNotificationId;
            NotificationType = data.Details.NotificationType;

            var selectedCodes = data.OperationCodes ?? new OperationCode[0];

            Codes =
                OperationCodeMetadata.GetCodesForOperation(data.Details.NotificationType)
                    .Select(c => new KeyValuePairViewModel<OperationCode, bool>(c, selectedCodes.Contains(c)))
                    .ToList();

            TechnologyEmployed = data.TechnologyEmployed;
        }

        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public IList<string> CodeDisplay
        {
            get { return Codes.Select(c => EnumHelper.GetDisplayName(c.Key)).ToList(); }
        }

        public IList<OperationCode> SelectedCodes
        {
            get { return Codes.Where(c => c.Value).Select(c => c.Key).ToList(); }
        }

        public IList<KeyValuePairViewModel<OperationCode, bool>> Codes { get; set; }

        [Display(Name = "TechnologyEmployed", ResourceType = typeof(UpdateWasteOperationViewModelResources))]
        [StringLength(70, ErrorMessageResourceName = "TechnologyEmployedMaxLength", ErrorMessageResourceType = typeof(UpdateWasteOperationViewModelResources))]
        public string TechnologyEmployed { get; set; }
    }
}
namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteOperation
{
    using Core.ImportNotification;
    using Core.ImportNotification.Draft;
    using Core.OperationCodes;
    using Core.Shared;
    using EA.Iws.Core.Extensions;
    using EA.Iws.Core.ImportNotificationAssessment;
    using Prsd.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Web.ViewModels.Shared;

    public class WasteOperationViewModel
    {
        public WasteOperationViewModel()
        {
        }

        public WasteOperationViewModel(NotificationDetails details, WasteOperation data, ImportInterimStatus interimStatus)
        {
            ImportNotificationId = details.ImportNotificationId;
            NotificationType = details.NotificationType;

            var selectedCodes = data.OperationCodes ?? new OperationCode[0];

            if (interimStatus.IsInterim ?? false)
            {
                Codes = OperationCodeMetadata.GetCodesForOperation(details.NotificationType)
                .Select(c => new KeyValuePairViewModel<OperationCode, bool>(c, selectedCodes.Contains(c)))
                .OrderByInterimsFirst(x => x.Key)
                .ToList();
            }
            else
            {
                Codes = OperationCodeMetadata.GetCodesForOperation(details.NotificationType)
                .Select(c => new KeyValuePairViewModel<OperationCode, bool>(c, selectedCodes.Contains(c)))
                .ToList();
            }
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

        [Display(Name = "TechnologyEmployed", ResourceType = typeof(WasteOperationViewModelResources))]
        [StringLength(70, ErrorMessageResourceName = "TechnologyEmployedMaxLength", ErrorMessageResourceType = typeof(WasteOperationViewModelResources))]
        public string TechnologyEmployed { get; set; }
  }
}
namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.ImportNotification;
    using Core.ImportNotification.Draft;
    using Core.OperationCodes;
    using Core.Shared;
    using Prsd.Core.Helpers;
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

            Codes =
                OperationCodeMetadata.GetCodesForOperation(details.NotificationType)
                    .Select(c => new KeyValuePairViewModel<OperationCode, bool>(c, data.OperationCodes.Contains(c)))
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

        [Display(Name = "TechnologyEmployed", ResourceType = typeof(WasteOperationViewModelResources))]
        public string TechnologyEmployed { get; set; }
    }
}
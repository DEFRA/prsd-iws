namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteComponent
{
    using EA.Iws.Core.ImportNotification;
    using EA.Iws.Core.ImportNotification.Draft;
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Web.ViewModels.Shared;
    using EA.Prsd.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WasteComponentViewModel
    {
        public WasteComponentViewModel()
        {
        }

        public WasteComponentViewModel(NotificationDetails details, WasteComponent data)
        {
            ImportNotificationId = details.ImportNotificationId;
            var selectedCodes = data.WasteComponentTypes ?? new WasteComponentType[0];

            Codes = WasteComponentMetadata.GetWasteComponents()
                                          .Select(c => new KeyValuePairViewModel<WasteComponentType, bool>(c, selectedCodes.Contains(c)))
                                          .ToList();
        }

        public Guid ImportNotificationId { get; set; }

        public IList<string> CodeDisplay
        {
            get { return Codes.Select(c => EnumHelper.GetDisplayName(c.Key)).ToList(); }
        }

        public IList<WasteComponentType> SelectedCodes
        {
            get { return Codes.Where(c => c.Value).Select(c => c.Key).ToList(); }
        }

        public IList<KeyValuePairViewModel<WasteComponentType, bool>> Codes { get; set; }
    }
}
namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.BaselOecdCode;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class BaselOecdCodeMap : IMap<WasteCodeDataAndNotificationData, BaselOecdCodeViewModel>,
        IMapWithParameter<BaselOecdCodeViewModel, Guid, SetBaselOecdCodeForNotification>
    {
        public BaselOecdCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            WasteCodeData selectedCode = null;

            if (source.NotificationWasteCodeData[CodeType.Basel].Length > 0)
            {
                selectedCode = source.NotificationWasteCodeData[CodeType.Basel].Single();
            }
            else if (source.NotificationWasteCodeData[CodeType.Oecd].Length > 0)
            {
                selectedCode = source.NotificationWasteCodeData[CodeType.Oecd].Single();
            }

            var model = new BaselOecdCodeViewModel
            {
                WasteCodes =
                    source.LookupWasteCodeData.Values.SelectMany(x => x)
                        .Select(wc => new WasteCodeViewModel
                        {
                            Id = wc.Id,
                            Description = wc.Description,
                            Name = wc.Code,
                            CodeType = wc.CodeType
                        })
                        .ToList()
            };

            if (selectedCode != null)
            {
                model.SelectedCode = selectedCode.Id;
            }

            if (source.NotApplicableCodes.Count > 0)
            {
                model.NotListed = true;
            }

            return model;
        }

        public SetBaselOecdCodeForNotification Map(BaselOecdCodeViewModel source, Guid parameter)
        {
            return new SetBaselOecdCodeForNotification(parameter,
                source.WasteCodes.Single(wc => wc.Id == source.SelectedCode.Value).CodeType,
                source.NotListed,
                source.SelectedCode.Value);
        }
    }
}
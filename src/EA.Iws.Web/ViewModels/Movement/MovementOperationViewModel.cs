namespace EA.Iws.Web.ViewModels.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.MovementOperation;
    using Core.Shared;
    using Shared;

    public class MovementOperationViewModel
    {
        public StringGuidRadioButtons RadioButtons { get; set; }

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public MovementOperationViewModel()
        {
        }

        public MovementOperationViewModel(Guid id, MovementOperationData model)
        {
            var list = model.MovementDatas;

            RadioButtons = new StringGuidRadioButtons(list
                .OrderBy(d => d.Number)
                .Select(d => new KeyValuePair<string, Guid>("Shipment " + d.Number, d.Id)));
            
            NotificationId = id;

            NotificationType = model.NotificationType;
        }
    }
}
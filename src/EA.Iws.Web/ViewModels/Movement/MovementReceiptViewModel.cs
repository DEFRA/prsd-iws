namespace EA.Iws.Web.ViewModels.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement;
    using Shared;

    public class MovementReceiptViewModel
    {
        public StringGuidRadioButtons RadioButtons { get; set; }

        public Guid NotificationId { get; set; }

        public MovementReceiptViewModel()
        {
        }

        public MovementReceiptViewModel(Guid id, IEnumerable<MovementData> model)
        {
            RadioButtons = new StringGuidRadioButtons(model
                .OrderBy(d => d.Number)
                .Select(d => new KeyValuePair<string, Guid>("Shipment " + d.Number, d.Id)));
            NotificationId = id;
        }
    }
}
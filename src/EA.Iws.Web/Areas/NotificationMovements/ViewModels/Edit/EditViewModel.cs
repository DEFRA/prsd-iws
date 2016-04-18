namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Edit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement;
    using Web.ViewModels.Shared;

    public class EditViewModel
    {
        public StringGuidRadioButtons Shipments { get; set; }

        public EditViewModel()
        {
        }

        public EditViewModel(IEnumerable<MovementData> movements)
        {
            Shipments = new StringGuidRadioButtons(movements
                .OrderBy(m => m.Number)
                .Select(m => 
                    new KeyValuePair<string, Guid>("Shipment " + m.Number, m.Id)));
        }
    }
}
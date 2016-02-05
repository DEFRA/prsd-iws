namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.UploadChoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement;
    using Web.ViewModels.Shared;

    public class UploadChoiceViewModel
    {
        public Guid NotificationId { get; set; }

        public StringGuidRadioButtons RadioButtons { get; set; }

        public UploadChoiceViewModel()
        {
        }

        public UploadChoiceViewModel(Guid id, IEnumerable<MovementData> model)
        {
            RadioButtons = new StringGuidRadioButtons(model
                .OrderBy(d => d.Number)
                .Select(d => new KeyValuePair<string, Guid>("Shipment " + d.Number, d.Id)));
            NotificationId = id;
        }

        public bool NoMovementsToList
        {
            get { return RadioButtons == null || RadioButtons.PossibleValues.Any(); }
        }
    }
}
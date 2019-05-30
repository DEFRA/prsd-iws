namespace EA.Iws.Web.Areas.ExportMovement.ViewModels.EditDate
{
    using System;
    using System.Collections.Generic;
    using Web.ViewModels.Shared;

    public class EditDateViewModel
    {
        public Guid NotificationId { get; set; }

        public IList<DateTime> DateEditHistory { get; set; }

        public DateInputViewmodel ActualDateofShipment { get; set; }

        public DateTime? AsDateTime()
        {
            return ActualDateofShipment.AsDateTime();
        }

        public EditDateViewModel()
        {
            ActualDateofShipment = new DateInputViewmodel();
        }
    }
}
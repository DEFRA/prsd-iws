namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.RejectedMovement
{
    using System;
    using Core.Movement;

    public class DetailsViewModel
    {
        public int Number { get; set; }

        public DateTime Date { get; set; }

        public string Reason { get; set; }

        public DetailsViewModel()
        {
        }

        public DetailsViewModel(RejectedMovementDetails details)
        {
            Number = details.Number;
            Date = details.Date;
            Reason = details.Reason;
        }
    }
}
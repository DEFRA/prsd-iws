namespace EA.Iws.Core.ImportMovement
{
    using System;

    public class ImportMovementDates
    {
        public int Number { get; set; }

        public DateTimeOffset ActualDate { get; set; }

        public DateTimeOffset? PreNotificationDate { get; set; }
    }
}

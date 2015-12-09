namespace EA.Iws.Core.ImportMovement
{
    using System;

    public class ImportMovementRecovery
    {
        public DateTimeOffset? OperationCompleteDate { get; set; }

        public bool IsOperationCompleted { get; set; }
    }
}

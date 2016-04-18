namespace EA.Iws.Core.ImportMovement
{
    using System;

    public class ImportMovementRecoveryData
    {
        public DateTimeOffset? OperationCompleteDate { get; set; }

        public bool IsOperationCompleted { get; set; }
    }
}

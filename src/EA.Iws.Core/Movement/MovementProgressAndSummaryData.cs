namespace EA.Iws.Core.Movement
{
    using System;

    public class MovementProgressAndSummaryData
    {
        public Guid NotificationId { get; set; }

        public Guid MovementId { get; set; }

        public string NotificationNumber { get; set; }

        public int TotalNumberOfMovements { get; set; }

        public int ThisMovementNumber { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int CurrentActiveLoads { get; set; }

        public ProgressData Progress { get; set; }
    }
}

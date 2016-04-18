namespace EA.Iws.Core.Movement
{
    using System;

    public class MovementSummary
    {
        public int MovementNumber { get; set; }

        public Guid MovementId { get; set; }

        public BasicMovementSummary SummaryData { get; set; }
    }
}
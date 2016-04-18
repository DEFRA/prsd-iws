namespace EA.Iws.Core.Movement
{
    public class ProposedUpdatedMovementDateResponse
    {
        public bool IsOutsideConsentPeriod { get; set; }

        public bool IsOutOfRange { get; set; }

        public bool IsOutOfRangeOfOriginalDate { get; set; }
    }
}
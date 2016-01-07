namespace EA.Iws.Core.Movement
{
    public class ProposedMovementDateResponse
    {
        public bool IsOutsideConsentPeriod { get; set; }

        public bool IsOutOfRange { get; set; }
    }
}
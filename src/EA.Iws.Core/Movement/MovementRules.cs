namespace EA.Iws.Core.Movement
{
    public enum MovementRules
    {
        TotalShipmentsReached = 1,
        TotalIntendedQuantityReached = 2,
        TotalIntendedQuantityExceeded = 3,
        ActiveLoadsReached = 4,
        ConsentPeriodExpired = 5,
        ConsentExpiresInFourWorkingDays = 6,
        ConsentExpiresInThreeOrLessWorkingDays = 7
    }
}
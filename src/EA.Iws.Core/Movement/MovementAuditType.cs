namespace EA.Iws.Core.Movement
{
    using System.ComponentModel.DataAnnotations;

    public enum MovementAuditType
    {
        Prenotified = 1,
        [Display(Name = "No prenotification received (internal user only change)")]
        NoPrenotificationReceived = 2,
        Received = 3,
        Recovered = 4,
        Disposed = 5,
        Rejected = 6,
        Cancelled = 7,
        Edited = 8,
        Deleted = 9
    }
}

namespace EA.Iws.Core.ImportMovement
{
    using System.ComponentModel.DataAnnotations;

    public enum ImportMovementStatus
    {
        New = 0,
        [Display(Name = "Prenotified")]
        Submitted = 1,
        Received = 2,
        Rejected = 3,
        Cancelled = 4,
        Complete = 5
    }
}
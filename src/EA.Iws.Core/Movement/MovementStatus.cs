namespace EA.Iws.Core.Movement
{
    using System.ComponentModel.DataAnnotations;

    public enum MovementStatus
    {
        New = 1,
        [Display(Name = "Prenotified")]
        Submitted = 2,
        Received = 3,
        Completed = 4,
        Rejected = 5,
        Cancelled = 6
    }
}
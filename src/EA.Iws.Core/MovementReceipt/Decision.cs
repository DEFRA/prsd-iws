namespace EA.Iws.Core.MovementReceipt
{
    using System.ComponentModel.DataAnnotations;

    public enum Decision
    {
        [Display(Name="Accepted")]
        Accepted = 1,
        [Display(Name="Rejected")]
        Rejected = 2
    }
}

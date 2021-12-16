namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;
    public enum StatsMarking
    {
        [Display(Name = "Illegal Shipment")]
        IllegalShipment  = 1,
        [Display(Name = "Did not proceed as intended")]
        BaselTable9 = 2,
        [Display(Name = "Accident occurred during transport")]
        BaselTable10 = 3
    }
}

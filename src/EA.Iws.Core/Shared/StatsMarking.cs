namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;
    public enum StatsMarking
    {
        [Display(Name = "Illegal Shipment (WSR Table 5)")]
        IllegalShipment  = 1,
        [Display(Name = "Did not proceed as intended (Basel Table 9)")]
        BaselTable9 = 2,
        [Display(Name = "Accident occurred during transport (Basel Table 10)")]
        BaselTable10 = 3
    }
}

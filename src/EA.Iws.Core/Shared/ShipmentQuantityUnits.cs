namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;

    public enum ShipmentQuantityUnits
    {
        [Display(Name = "Tonnes(Mg)")]
        Tonnes = 1,

        [Display(Name = "m3")]
        CubicMetres = 2,

        [Display(Name = "Kgs")]
        Kilograms = 3,

        [Display(Name = "Ltrs")]
        Litres = 4
    }
}

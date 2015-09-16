namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;

    public enum ShipmentQuantityUnits
    {
        [Display(Name = "tonnes (t)", ShortName = "t")]
        Tonnes = 1,

        [Display(Name = "cubic metres (m\u00B3)", ShortName = "m\u00B3")]
        CubicMetres = 2,

        [Display(Name = "kilograms (kg)", ShortName = "kg")]
        Kilograms = 3,

        [Display(Name = "litres (l)", ShortName = "l")]
        Litres = 4
    }
}

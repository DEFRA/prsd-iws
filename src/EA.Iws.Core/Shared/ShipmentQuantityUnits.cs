namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;

    public enum ShipmentQuantityUnits
    {
        [Display(Name = "tonnes (t)")]
        Tonnes = 1,

        [Display(Name = "cubic metres (m\u00B3)")]
        CubicMetres = 2,

        [Display(Name = "kilograms (kg)")]
        Kilograms = 3,

        [Display(Name = "litres (l)")]
        Litres = 4
    }
}

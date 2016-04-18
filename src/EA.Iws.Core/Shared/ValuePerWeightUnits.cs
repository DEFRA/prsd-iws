namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;

    public enum ValuePerWeightUnits
    {
        [Display(Name = "per kg")]
        Kilogram = 1,

        [Display(Name = "per tonne")]
        Tonne = 2
    }
}

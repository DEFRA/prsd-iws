namespace EA.Iws.Core.WasteType
{
    using System.ComponentModel.DataAnnotations;

    public enum WasteCategoryType
    {
        [Display(Name = "Metals")]
        Metals = 1,

        [Display(Name = "Catalysts")]
        Catalysts = 2,

        [Display(Name = "WEEE")]
        WEEE = 3,

        [Display(Name = "Plastics")]
        Plastics = 4,

        [Display(Name = "Batteries")]
        Batteries = 5,

        [Display(Name = "Clinical")]
        Clinical = 6,

        [Display(Name = "Pharmaceuticals")]
        Pharmaceuticals = 7,

        [Display(Name = "Rags/absorbents")]
        Ragsabsorbents = 8,

        [Display(Name = "Oils")]
        Oils = 9,

        [Display(Name = "Solvents/dyes")]
        Solventsdyes = 10,

        [Display(Name = "Single ship")]
        Singleship = 11,

        [Display(Name = "Platform/rig")]
        Platformrig = 12,

        [Display(Name = "Not applicable")]
        Notapplicable = 13
    }
}

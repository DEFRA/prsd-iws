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

        [Display(Name = "Pharamaceuticals")]
        Pharamaceuticals = 7,

        [Display(Name = "Rugs/Absorbents")]
        RugsAbsorbents = 8,

        [Display(Name = "Oils")]
        Oils = 9,

        [Display(Name = "Solvents/Dyes")]
        SolventsDyes = 10,

        [Display(Name = "Single ship")]
        Singleship = 11,

        [Display(Name = "Platform/Rig")]
        PlatformRig = 12,

        [Display(Name = "N/A")]
        NA = 13
    }
}

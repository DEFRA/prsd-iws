namespace EA.Iws.Core.WasteType
{
    using System.ComponentModel.DataAnnotations;
    public enum ChemicalCompositionCategory
    {
        [Display(Name = "Other")]
        Other = 0,
        [Display(Name = "Paper")]
        Paper = 1,
        [Display(Name = "Plastics")]
        Plastics = 2,
        [Display(Name = "Food")]
        Food = 3,
        [Display(Name = "Wood")]
        Wood = 4,
        [Display(Name = "Textiles")]
        Textiles = 5,
        [Display(Name = "Metals")]
        Metals = 6
    }
}
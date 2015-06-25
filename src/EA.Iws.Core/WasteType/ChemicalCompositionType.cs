namespace EA.Iws.Core.WasteType
{
    using System.ComponentModel.DataAnnotations;

    public enum ChemicalCompositionType
    {
        [Display(Name = "Refuse derived fuel (RDF)")]
        RDF = 1,
        [Display(Name = "Solid recovered fuel (SRF)")]
        SRF = 2,
        [Display(Name = "Wood")]
        Wood = 3,
        [Display(Name = "Other")]
        Other = 4
    }
}
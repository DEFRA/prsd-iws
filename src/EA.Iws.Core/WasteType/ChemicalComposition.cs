namespace EA.Iws.Core.WasteType
{
    using System.ComponentModel.DataAnnotations;

    public enum ChemicalComposition
    {
        [Display(Name = "Refuse derived fuel (RDF)", ShortName = "RDF")]
        RDF = 1,
        [Display(Name = "Solid recovered fuel (SRF)", ShortName = "SRF")]
        SRF = 2,
        [Display(Name = "Wood", ShortName = "Wood")]
        Wood = 3,
        [Display(Name = "Other", ShortName = "Other")]
        Other = 4
    }
}
namespace EA.Iws.Core.WasteCodes
{
    using System.ComponentModel.DataAnnotations;

    public enum CodeType
    {
        [Display(Name = "Basel")] Basel = 1,
        [Display(Name = "OECD")] Oecd = 2,
        [Display(Name = "EWC")] Ewc = 3,
        [Display(Name = "Y-code")] Y = 4,
        [Display(Name = "H-code")] H = 5,
        [Display(Name = "UN class")] Un = 6,
        [Display(Name = "Export Code")] ExportCode = 7,
        [Display(Name = "Import Code")] ImportCode = 8,
        [Display(Name = "Other Code")] OtherCode = 9,
        [Display(Name = "Customs Code")] CustomsCode = 10,
        [Display(Name = "UN Number Code")] UnNumber = 11
    }
}
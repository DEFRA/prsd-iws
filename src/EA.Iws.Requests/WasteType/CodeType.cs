namespace EA.Iws.Requests.WasteType
{
    using System.ComponentModel.DataAnnotations;

    public enum CodeType
    {
        [Display(Name = "Basel")]
        Basel = 1,
        [Display(Name = "OECD")]
        Oecd = 2,
       [Display(Name = "EWC")]
        Ewc = 3
    }
}
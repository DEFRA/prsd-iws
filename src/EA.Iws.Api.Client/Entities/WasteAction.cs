namespace EA.Iws.Api.Client.Entities
{
    using System.ComponentModel.DataAnnotations;

    public enum WasteAction
    {
        [Display(Name = "Recovery")]
        Recovery = 1,
        [Display(Name = "Disposal")]
        Disposal = 2
    }
}
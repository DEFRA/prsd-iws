namespace EA.Iws.Requests.Notification
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
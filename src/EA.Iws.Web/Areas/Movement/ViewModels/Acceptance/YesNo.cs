namespace EA.Iws.Web.Areas.Movement.ViewModels.Acceptance
{
    using System.ComponentModel.DataAnnotations;

    public enum YesNo
    {
        [Display(Name = "Yes")]
        Yes = 1,

        [Display(Name = "No")]
        No = 2
    }
}
namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum TextFieldOperator
    {
        [Display(Name = "Contains")]
        Contains,

        [Display(Name = "Does not contain")]
        DoesNotContain
    }
}

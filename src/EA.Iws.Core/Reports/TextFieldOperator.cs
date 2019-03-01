namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum TextFieldOperator
    {
        [Display(Name = "Starting with")]
        StartsWith,

        [Display(Name = "Contains")]
        Contains,

        [Display(Name = "Does not contain")]
        DoesNotContain,

        [Display(Name = "Equal to")]
        EqualsTo
    }
}

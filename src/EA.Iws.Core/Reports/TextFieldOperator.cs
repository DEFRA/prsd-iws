namespace EA.Iws.Core.Reports
{
    using System.ComponentModel.DataAnnotations;

    public enum TextFieldOperator
    {
        [Display(Name = "Starting with")]
        StartsWith,

        [Display(Name = "Contains")]
        Contains,

        [Display(Name = "Equal to")]
        EqualsTo,

        [Display(Name = "Does not contain")]
        DoesNotContain
    }
}

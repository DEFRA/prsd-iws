namespace EA.Iws.Core.Authorization
{
    using System.ComponentModel.DataAnnotations;

    public enum UserRole
    {
        External,
        [Display(Name = "Standard user")]
        Internal,
        Administrator
    }
}
namespace EA.Iws.Core.Notification.AdditionalCharge
{
    using System.ComponentModel.DataAnnotations;

    public enum AdditionalChargeType
    {
        [Display(Name = "Export details", Description = "Export details")]
        Export = 1,

        [Display(Name = "Producer details", Description = "Producer details")]
        Producer = 2,

        [Display(Name = "Importer details", Description = "Importer details")]
        Importer = 3,

        [Display(Name = "Consignee details", Description = "Consignee details")]
        Consignee = 4,
    }
}

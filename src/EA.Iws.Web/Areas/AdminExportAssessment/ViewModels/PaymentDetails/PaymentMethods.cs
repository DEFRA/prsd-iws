namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.PaymentDetails
{
    using System.ComponentModel.DataAnnotations;

    public enum PaymentMethods
    {
        [Display(Name = "Cheque")]
        Cheque = 0,

        [Display(Name = "BACS / CHAPS")]
        BacsChaps = 1,

        [Display(Name = "Credit Card")]
        Card = 2,

        [Display(Name = "Postal order")]
        PostalOrder = 3
    }
}
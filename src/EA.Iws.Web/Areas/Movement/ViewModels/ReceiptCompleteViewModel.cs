namespace EA.Iws.Web.Areas.Movement.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class ReceiptCompleteViewModel
    {
        public Guid NotificationId { get; set; }

        [Display(Name = "Upload the signed copy of the certificate of receipt")]
        public HttpPostedFileBase File { get; set; }
    }
}
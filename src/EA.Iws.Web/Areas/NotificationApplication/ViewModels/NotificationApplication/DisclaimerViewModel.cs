namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Prsd.Core.Validation;

    public class DisclaimerViewModel
    {
        public Guid Id { get; set; }

        [MustBeTrue(ErrorMessage = "Please confirm that you have read standard data notice and disclaimer")]
        public bool TermsAndConditions { get; set; }
    }
}
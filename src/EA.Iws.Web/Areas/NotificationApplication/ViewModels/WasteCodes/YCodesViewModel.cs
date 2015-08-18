namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;

    public class YCodesViewModel
    {
        public Guid NotificationId { get; set; }

        public EnterWasteCodesViewModel EnterWasteCodesViewModel { get; set; }

        public YCodesViewModel()
        {
            EnterWasteCodesViewModel = new EnterWasteCodesViewModel();
        }
    }
}
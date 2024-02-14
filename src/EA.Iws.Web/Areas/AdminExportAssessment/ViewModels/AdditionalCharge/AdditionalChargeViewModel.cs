namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.AdditionalCharge
{
    using EA.Iws.Core.Notification.AdditionalCharge;
    using System.Collections.Generic;

    public class AdditionalChargeViewModel
    {
        public AdditionalChargeViewModel()
        {
        }        

        public IList<NotificationAdditionalChargeForDisplay> AdditionalChargeData { get; set; }
    }
}
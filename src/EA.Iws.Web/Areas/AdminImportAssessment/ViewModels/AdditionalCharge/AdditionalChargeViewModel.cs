namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.AdditionalCharge
{
    using EA.Iws.Core.ImportNotification.AdditionalCharge;
    using System.Collections.Generic;

    public class AdditionalChargeViewModel
    {
        public AdditionalChargeViewModel()
        {
        }

        public IList<AdditionalChargeForDisplay> AdditionalChargeData { get; set; }
    }
}
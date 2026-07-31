namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using EA.Iws.Core.NotificationAssessment;

    public class AnnexPlusViewModel
    {
        public AnnexViewModel Annex { get; }
        public NotificationStatus Status { get; }

        public AnnexPlusViewModel(AnnexViewModel annex, NotificationStatus status) 
        { 
            Annex = annex;
            Status = status;
        }
    }
}
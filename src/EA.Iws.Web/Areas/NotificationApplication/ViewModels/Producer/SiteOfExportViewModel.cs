namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using System.Collections.Generic;
    using Core.Producers;

    public class SiteOfExportViewModel
    {
        public Guid NotificationId { get; set; }

        public Guid SelectedSiteOfExport { get; set; }

        public IList<ProducerData> Producers { get; set; }

        public SiteOfExportViewModel()
        {
            Producers = new List<ProducerData>();
        }
    }
}
namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using System.Collections.Generic;
    using Core.Producers;
    using Requests.Producers;

    public class MultipleProducersViewModel
    {
        public MultipleProducersViewModel()
        {
            ProducerData = new List<ProducerData>();
        }

        public Guid NotificationId { get; set; }

        public List<ProducerData> ProducerData { get; set; }

        public bool HasSiteOfExport { get; set; }

        public string SelectedValue { get; set; }
    }
}
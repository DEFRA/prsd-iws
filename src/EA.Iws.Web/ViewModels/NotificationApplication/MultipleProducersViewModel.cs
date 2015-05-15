namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using Requests.Notification;

    public class MultipleProducersViewModel
    {
        public MultipleProducersViewModel()
        {
            ProducerData = new List<ProducerData>();
        }

        public Guid NotificationId { get; set; }

        public List<ProducerData> ProducerData { get; set; }
    }
}
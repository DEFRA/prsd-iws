namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;

    public class RemoveProducerViewModel
    {
        public Guid NotificationId { get; set; }

        public Guid ProducerId { get; set; }

        public string ProducerName { get; set; }
    }
}
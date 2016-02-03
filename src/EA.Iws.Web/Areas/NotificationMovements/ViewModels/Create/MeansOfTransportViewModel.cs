namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using Core.MeansOfTransport;

    public class MeansOfTransportViewModel
    {
        public IList<TransportMethod> NotificationMeansOfTransport { get; set; }
    }
}
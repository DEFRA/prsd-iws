namespace EA.Iws.Requests.Exporters
{
    using System;
    using Prsd.Core.Mediator;

    public class AddExporterToNotification : IRequest<Guid>
    {
        public ExporterData Exporter { get; private set; }

        public AddExporterToNotification(ExporterData exporter)
        {
            this.Exporter = exporter;
        }
    }
}

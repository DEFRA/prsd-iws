namespace EA.Iws.Requests.Importer
{
    using System;
    using Prsd.Core.Mediator;

    public class AddImporterToNotification : IRequest<Guid>
    {
        public ImporterData Importer { get; private set; }

        public AddImporterToNotification(ImporterData importer)
        {
            Importer = importer;
        }
    }
}

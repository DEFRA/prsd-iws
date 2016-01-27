namespace EA.Iws.RequestHandlers.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.Admin;
    using Domain = Domain.NotificationApplication;

    internal class ExportNotificationOwnerDisplayMap : IMap<Domain.ExportNotificationOwnerDisplay, Core.ExportNotificationOwnerDisplay>
    {
        public Core.ExportNotificationOwnerDisplay Map(Domain.ExportNotificationOwnerDisplay source)
        {
            return new Core.ExportNotificationOwnerDisplay
            {
                NotificationId = source.NotificationId,
                Number = source.Number,
                ExporterName = source.ExporterName,
                ImporterName = source.ImporterName,
                ProducerName = source.ProducerName,
                OwnerName = source.OwnerName
            };
        }
    }
}
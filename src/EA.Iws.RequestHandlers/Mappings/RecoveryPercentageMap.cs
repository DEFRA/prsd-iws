namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class RecoveryPercentageMap : IMap<NotificationApplication, RecoveryPercentageData>
    {
        public RecoveryPercentageData Map(NotificationApplication source)
        {
            RecoveryPercentageData data = new RecoveryPercentageData
            {
                NotificationId = source.Id,
                IsProvidedByImporter = source.IsProvidedByImporter,
                PercentageRecoverable = source.PercentageRecoverable,
                MethodOfDisposal = source.MethodOfDisposal
            };
             
            return data;
        }
    }
}

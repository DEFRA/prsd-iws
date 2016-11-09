namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class NotificationDatesMap : IMap<NotificationDatesSummary, NotificationDatesData>
    {
        public NotificationDatesData Map(NotificationDatesSummary source)
        {
            return new NotificationDatesData
            {
                CurrentStatus = source.CurrentStatus,
                NotificationId = source.NotificationId,
                NotificationReceivedDate = source.NotificationReceivedDate,
                PaymentReceivedDate = source.PaymentReceivedDate,
                PaymentIsComplete = source.PaymentIsComplete,
                CommencementDate = source.CommencementDate,
                NameOfOfficer = source.NameOfOfficer,
                CompletedDate = source.CompletedDate,
                TransmittedDate = source.TransmittedDate,
                AcknowledgedDate = source.AcknowledgedDate,
                DecisionRequiredDate = source.DecisionRequiredDate,
                FileClosedDate = source.FileClosedDate,
                ArchiveReference = source.ArchiveReference
            };
        }
    }
}
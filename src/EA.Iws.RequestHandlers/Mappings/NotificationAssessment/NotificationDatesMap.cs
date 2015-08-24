namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class NotificationDatesMap : IMapWithParameter<NotificationDates, Guid, NotificationDatesData>
    {
        public NotificationDatesData Map(NotificationDates source, Guid parameter)
        {
            return new NotificationDatesData
            {
                NotificationId = parameter,
                NotificationReceivedDate = source.NotificationReceivedDate,
                PaymentReceivedDate = source.PaymentReceivedDate,
                CommencementDate = source.CommencementDate,
                NameOfOfficer = source.NameOfOfficer,
                CompletedDate = source.CompleteDate
            };
        }
    }
}
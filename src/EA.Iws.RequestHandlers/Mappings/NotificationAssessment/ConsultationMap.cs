namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    internal class ConsultationMap : IMap<Consultation, ConsultationData>
    {
        public ConsultationData Map(Consultation source)
        {
            if (source == null)
            {
                return new ConsultationData();
            }

            return new ConsultationData
            {
                LocalAreaId = source.LocalAreaId
            };
        }
    }
}
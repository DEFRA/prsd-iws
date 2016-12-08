namespace EA.Iws.RequestHandlers.Mappings.ImportNotificationAssessment
{
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment;
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
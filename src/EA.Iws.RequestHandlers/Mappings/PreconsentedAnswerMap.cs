namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class PreconsentedAnswerMap : IMap<NotificationApplication, string>
    {
        private readonly IFacilityRepository facilityRepository;

        public PreconsentedAnswerMap(IFacilityRepository facilityRepository)
        {
            this.facilityRepository = facilityRepository;
        }

        public string Map(NotificationApplication source)
        {
            string preconsentedAnswer = string.Empty;

            var facilityCollection = Task.Run(() => facilityRepository.GetByNotificationId(source.Id)).Result;

            if (facilityCollection.AllFacilitiesPreconsented.HasValue)
            {
                preconsentedAnswer = facilityCollection.AllFacilitiesPreconsented.GetValueOrDefault()
                    ? "Yes"
                    : "No";
            }

            return preconsentedAnswer;
        }
    }
}
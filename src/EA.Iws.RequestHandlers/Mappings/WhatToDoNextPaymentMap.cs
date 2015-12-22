namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class WhatToDoNextPaymentMap : IMapWithParameter<NotificationApplication, UnitedKingdomCompetentAuthority, WhatToDoNextPaymentData>
    {
        private readonly IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> competentAuthorityMap;

        public WhatToDoNextPaymentMap(IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> competentAuthorityMap)
        {
            this.competentAuthorityMap = competentAuthorityMap;
        }

        public WhatToDoNextPaymentData Map(NotificationApplication source, UnitedKingdomCompetentAuthority parameter)
        {
            return new WhatToDoNextPaymentData
            {
                Id = source.Id,
                NotificationNumber = source.NotificationNumber,
                NotificationType = source.NotificationType,
                CompetentAuthority = (Core.Notification.CompetentAuthority)source.CompetentAuthority.Value,
                UnitedKingdomCompetentAuthorityData = competentAuthorityMap.Map(parameter)
            };
        }
    }
}

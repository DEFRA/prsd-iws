namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class WhatToDoNextMap : IMapWithParameter<NotificationApplication, UnitedKingdomCompetentAuthority, WhatToDoNextData>
    {
        private readonly IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> competentAuthorityMap;

        public WhatToDoNextMap(IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> competentAuthorityMap)
        {
            this.competentAuthorityMap = competentAuthorityMap;
        }

        public WhatToDoNextData Map(NotificationApplication source, UnitedKingdomCompetentAuthority parameter)
        {
            return new WhatToDoNextData
            {
                Id = source.Id,
                NotificationNumber = source.NotificationNumber,
                NotificationType = (Core.Shared.NotificationType)source.NotificationType.Value,
                CompetentAuthority = (Core.Notification.CompetentAuthority)source.CompetentAuthority.Value,
                UnitedKingdomCompetentAuthorityData = competentAuthorityMap.Map(parameter)
            };
        }
    }
}

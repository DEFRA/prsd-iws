namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.TechnologyEmployed;
    using Domain.Notification;
    using Prsd.Core.Mapper;

    internal class TechnologyEmployedMap : IMap<NotificationApplication, TechnologyEmployedData>
    {
        public TechnologyEmployedData Map(NotificationApplication source)
        {
            if (source.HasTechnologyEmployed)
            {
                return new TechnologyEmployedData
                {
                    AnnexProvided = source.TechnologyEmployed.AnnexProvided,
                    Details = source.TechnologyEmployed.Details,
                    NotificationId = source.Id,
                    FurtherDetails = source.TechnologyEmployed.FurtherDetails,
                    HasTechnologyEmployed = true
                };
            }
            else
            {
                return new TechnologyEmployedData
                {
                    NotificationId = source.Id,
                    HasTechnologyEmployed = false
                };
            }
        }
    }
}
namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class SpecialHandlingMap : IMap<NotificationApplication, SpecialHandlingData>
    {
        public SpecialHandlingData Map(NotificationApplication source)
        {
            return new SpecialHandlingData
            {
                NotificationId = source.Id,
                HasSpecialHandlingRequirements = source.HasSpecialHandlingRequirements,
                SpecialHandlingDetails = source.SpecialHandlingDetails
            };
        }
    }
}
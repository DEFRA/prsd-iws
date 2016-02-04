namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Carriers;
    using Core.MeansOfTransport;
    using Core.Notification.Overview;
    using Core.PackagingType;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;

    internal class TransportationInfoMap : IMap<NotificationApplication, Transportation>
    {
        private readonly IMeansOfTransportRepository repository;
        private readonly IMap<NotificationApplication, IList<CarrierData>> carrierMap;

        public TransportationInfoMap(IMeansOfTransportRepository repository,
            IMap<NotificationApplication, IList<CarrierData>> carrierMap)
        {
            this.carrierMap = carrierMap;
            this.repository = repository;
        }

        public Transportation Map(NotificationApplication notification)
        {
            var meansOfTransport = Task.Run(() => repository.GetByNotificationId(notification.Id)).Result;
                        
            return new Transportation
            {
                NotificationId = notification.Id,
                Carriers = carrierMap.Map(notification).ToList(),
                MeanOfTransport = meansOfTransport != null ? meansOfTransport.Route.ToList() : new List<TransportMethod>(),
                PackagingData = GetPackagingData(notification),
                SpecialHandlingDetails = GetSpecialHandling(notification)
            };
        }

        private static List<string> GetPackagingData(NotificationApplication notification)
        {
            var packagingData = new List<string>();
            foreach (var packagingInfo in notification.PackagingInfos)
            {
                packagingData.Add(packagingInfo.PackagingType != PackagingType.Other
                    ? (int)packagingInfo.PackagingType + " - " + EnumHelper.GetDisplayName(packagingInfo.PackagingType)
                    : packagingInfo.OtherDescription);
            }
            packagingData.Sort();

            return packagingData;
        }

        private static string GetSpecialHandling(NotificationApplication notification)
        {
            var specialHandlingAnswer = string.Empty;
            if (notification.HasSpecialHandlingRequirements.HasValue)
            {
                specialHandlingAnswer = notification.HasSpecialHandlingRequirements.Value
                    ? notification.SpecialHandlingDetails
                    : "No special handling required";
            }
            return specialHandlingAnswer;
        }
    }
}
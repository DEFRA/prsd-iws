namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Carriers;
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class TransportationInfoMap : IMap<NotificationApplication, TransportationInfo>
    {
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper;
        private readonly IMap<NotificationApplication, IList<CarrierData>> carrierMap;

        public TransportationInfoMap(
            IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper,
            IMap<NotificationApplication, IList<CarrierData>> carrierMap)
        {
            this.completionProgressMapper = completionProgressMapper;
            this.carrierMap = carrierMap;
        }

        public TransportationInfo Map(NotificationApplication notification)
        {
            return new TransportationInfo 
            {
                Progress = completionProgressMapper.Map(notification),
                Carriers = carrierMap.Map(notification).ToList(),
                MeanOfTransport = notification.MeansOfTransport.ToList(),
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
                    ? packagingInfo.PackagingType.Value + " - " + packagingInfo.PackagingType.DisplayName
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

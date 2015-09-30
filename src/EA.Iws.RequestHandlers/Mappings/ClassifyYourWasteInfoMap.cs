namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification.Overview;
    using PhysicalCharacteristicType = Domain.NotificationApplication.PhysicalCharacteristicType;

    internal class ClassifyYourWasteInfoMap : IMap<NotificationApplication, ClassifyYourWaste>
    {
        private readonly IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> wasteCodesMapper;
        private readonly IMap<WasteType, WasteTypeData> wasteTypeMapper;

        public ClassifyYourWasteInfoMap(
            IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> wasteCodesMapper,
            IMap<WasteType, WasteTypeData> wasteTypeMapper)
        {
            this.wasteCodesMapper = wasteCodesMapper;
            this.wasteTypeMapper = wasteTypeMapper;
        }

        public ClassifyYourWaste Map(NotificationApplication notification)
        {
            return new ClassifyYourWaste
            {
                NotificationId = notification.Id,
                ChemicalComposition = GetWasteType(notification),
                PhysicalCharacteristics = GetPhysicalCharacteristics(notification),
                ProcessOfGeneration = GetWasteGenerationProcess(notification),
            };
        }

        private WasteTypeData GetWasteType(NotificationApplication notification)
        {
            WasteTypeData chemicalComposition = null;
            if (notification.WasteType != null)
            {
                chemicalComposition = wasteTypeMapper.Map(notification.WasteType);
            }
            return chemicalComposition;
        }

        private static List<string> GetPhysicalCharacteristics(NotificationApplication notification)
        {
            var physicalCharacteristicsData = new List<string>();
            foreach (var c in notification.PhysicalCharacteristics)
            {
                physicalCharacteristicsData.Add(c.PhysicalCharacteristic != PhysicalCharacteristicType.Other
                    ? c.PhysicalCharacteristic.DisplayName
                    : c.OtherDescription);
            }
            return physicalCharacteristicsData;
        }

        private static WasteGenerationProcessData GetWasteGenerationProcess(NotificationApplication notification)
        {
            var wasteGenerationData = new WasteGenerationProcessData
            {
                Process = notification.WasteGenerationProcess ?? string.Empty,
                IsDocumentAttached = notification.IsWasteGenerationProcessAttached ?? false
            };
            return wasteGenerationData;
        }
    }
}

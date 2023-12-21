﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Notification.Overview;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;

    internal class ClassifyYourWasteInfoMap : IMap<NotificationApplication, WasteClassificationOverview>
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

        public WasteClassificationOverview Map(NotificationApplication notification)
        {
            return new WasteClassificationOverview
            {
                NotificationId = notification.Id,
                ChemicalComposition = GetWasteType(notification),
                PhysicalCharacteristics = GetPhysicalCharacteristics(notification),
                ProcessOfGeneration = GetWasteGenerationProcess(notification),
                WasteComponentTypes = GetWasteComponentTypes(notification)
            };
        }

        private List<string> GetWasteComponentTypes(NotificationApplication notification)
        {
            var wasteComponentTypes = new List<string>();
            foreach (var c in notification.WasteComponentInfos)
            {
                wasteComponentTypes.Add(EnumHelper.GetDisplayName(c.WasteComponentType));
            }
            return wasteComponentTypes;
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
                    ? EnumHelper.GetDisplayName(c.PhysicalCharacteristic)
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

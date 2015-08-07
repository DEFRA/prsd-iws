namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;
    using PhysicalCharacteristicType = Domain.NotificationApplication.PhysicalCharacteristicType;

    internal class ClassifyYourWasteInfoMap : IMap<NotificationApplication, ClassifyYourWasteInfo>
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

        public ClassifyYourWasteInfo Map(NotificationApplication notification)
        {
            return new ClassifyYourWasteInfo
            {
                NotificationId = notification.Id,
                ChemicalComposition = GetWasteType(notification),
                PhysicalCharacteristics = GetPhysicalCharacteristics(notification),
                ProcessOfGeneration = GetWasteGenerationProcess(notification),
                BaselOecdCode = wasteCodesMapper.Map(notification, CodeType.Basel),
                EwcCodes = wasteCodesMapper.Map(notification, CodeType.Ewc),
                NationExportCode = wasteCodesMapper.Map(notification, CodeType.ExportCode),
                NationImportCode = wasteCodesMapper.Map(notification, CodeType.ImportCode),
                OtherCodes = wasteCodesMapper.Map(notification, CodeType.OtherCode),
                YCodes = wasteCodesMapper.Map(notification, CodeType.Y),
                HCodes = wasteCodesMapper.Map(notification, CodeType.H),
                UnClass = wasteCodesMapper.Map(notification, CodeType.Un),
                UnNumber = wasteCodesMapper.Map(notification, CodeType.UnNumber),
                CustomCodes = wasteCodesMapper.Map(notification, CodeType.CustomsCode)
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

namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.RecoveryInfo;
    using Core.Shipment;
    using Core.TechnologyEmployed;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.CustomsOffice;
    using Requests.Notification;
    using Requests.StateOfExport;
    using PhysicalCharacteristicType = Domain.Notification.PhysicalCharacteristicType;

    internal class NotificationInfoMap : IMap<NotificationApplication, NotificationInfo>
    {
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper;
        private readonly IMap<NotificationApplication, StateOfExportWithTransportRouteData> transportRouteMap;
        private readonly IMap<NotificationApplication, EntryCustomsOfficeAddData> customsOfficeEntryMap;
        private readonly IMap<NotificationApplication, ExitCustomsOfficeAddData> customsOfficeExitMap;
        private readonly IMap<NotificationApplication, ShipmentData> shipmentMapper;
        private readonly IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> wasteCodesMapper;
        private readonly IMap<WasteType, WasteTypeData> wasteTypeMapper;
        private readonly IMap<NotificationApplication, string> preconsentedAnswerMap;

        public NotificationInfoMap(IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper,
            IMap<NotificationApplication, StateOfExportWithTransportRouteData> transportRouteMap,
            IMap<NotificationApplication, EntryCustomsOfficeAddData> customsOfficeEntryMap,
            IMap<NotificationApplication, ExitCustomsOfficeAddData> customsOfficeExitMap,
            IMap<NotificationApplication, ShipmentData> shipmentMapper,
            IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> wasteCodesMapper,
            IMap<WasteType, WasteTypeData> wasteTypeMapper,
            IMap<NotificationApplication, string> preconsentedAnswerMap)
        {
            this.completionProgressMapper = completionProgressMapper;
            this.transportRouteMap = transportRouteMap;
            this.customsOfficeEntryMap = customsOfficeEntryMap;
            this.customsOfficeExitMap = customsOfficeExitMap;
            this.shipmentMapper = shipmentMapper;
            this.wasteCodesMapper = wasteCodesMapper;
            this.wasteTypeMapper = wasteTypeMapper;
            this.preconsentedAnswerMap = preconsentedAnswerMap;
        }

        public NotificationInfo Map(NotificationApplication notification)
        {
            List<string> producersCompanyNames = notification.Producers.Select(p => p != null ? p.Business.Name : null).ToList();
            List<string> facilitiesCompanyNames = notification.Facilities.Select(f => f != null ? f.Business.Name : null).ToList();
            List<string> carriersCompanyNames = notification.Carriers.Select(c => c != null ? c.Business.Name : null).ToList();

            var operationCodes = GetOperationCodes(notification);
            var packagingData = GetPackagingData(notification);
            var technologyEmployed = GetTechnologyEmployed(notification);
            var specialHandlingAnswer = GetSpecialHandling(notification);

            var transportRouteData = transportRouteMap.Map(notification);
            var entryCustomsOfficeData = customsOfficeEntryMap.Map(notification);
            var exitCustomsOfficeData = customsOfficeExitMap.Map(notification);
            var shipmentData = shipmentMapper.Map(notification);

            var chemicalComposition = GetWasteType(notification);
            var wasteGenerationData = GetWasteGenerationProcess(notification);
            var physicalCharacteristicsData = GetPhysicalCharacteristics(notification);

            var baselOecdCodeData = wasteCodesMapper.Map(notification, CodeType.Basel);
            var ewcCodesData = wasteCodesMapper.Map(notification, CodeType.Ewc);
            var nationExportCodeData = wasteCodesMapper.Map(notification, CodeType.ExportCode);
            var nationImportCodeData = wasteCodesMapper.Map(notification, CodeType.ImportCode);
            var otherCodesData = wasteCodesMapper.Map(notification, CodeType.OtherCode);
            var ycodesData = wasteCodesMapper.Map(notification, CodeType.Y);
            var hcodesData = wasteCodesMapper.Map(notification, CodeType.H);
            var unclassData = wasteCodesMapper.Map(notification, CodeType.Un);
            var unnumberData = wasteCodesMapper.Map(notification, CodeType.UnNumber);
            var customCodesData = wasteCodesMapper.Map(notification, CodeType.CustomsCode);

            var recoveryPercentageData = GetRecoveryPercentage(notification);
            var recoveryInfoData = GetRecoveryInfo(notification);

            return new NotificationInfo
            {
                NotificationId = notification.Id,
                CompetentAuthority = (CompetentAuthority)notification.CompetentAuthority.Value,
                NotificationNumber = notification.NotificationNumber,
                NotificationType =
                    notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                Progress = completionProgressMapper.Map(notification),
                ExporterCompanyName = notification.Exporter != null ? notification.Exporter.Business.Name : string.Empty,
                ProducersCompanyNames = producersCompanyNames,
                ImporterCompanyName = notification.Importer != null ? notification.Importer.Business.Name : string.Empty,
                FacilitiesCompanyNames = facilitiesCompanyNames,
                PreconstedAnswer = preconsentedAnswerMap.Map(notification),
                OperationCodes = operationCodes,
                TechnologyEmployed = technologyEmployed,
                ReasonForExport = notification.ReasonForExport ?? string.Empty,
                CariersCompanyNames = carriersCompanyNames,
                MeanOfTransport = notification.MeansOfTransport.ToList(),
                PackagingData = packagingData,
                SpecialHandlingDetails = specialHandlingAnswer,
                TransportRoute = transportRouteData,
                EntryCustomsOffice = entryCustomsOfficeData,
                ExitCustomsOffice = exitCustomsOfficeData,
                ShipmentData = shipmentData,
                ChemicalComposition = chemicalComposition,
                ProcessOfGeneration = wasteGenerationData,
                PhysicalCharacteristics = physicalCharacteristicsData,
                BaselOecdCode = baselOecdCodeData,
                EwcCodes = ewcCodesData,
                NationExportCode = nationExportCodeData,
                NationImportCode = nationImportCodeData,
                OtherCodes = otherCodesData,
                YCodes = ycodesData,
                HCodes = hcodesData,
                UnClass = unclassData,
                UnNumber = unnumberData,
                CustomCodes = customCodesData,
                RecoveryPercentageData = recoveryPercentageData,
                RecoveryInfoData = recoveryInfoData
            };
        }

        private static RecoveryInfoData GetRecoveryInfo(NotificationApplication notification)
        {
            var recoveryInfoData = new RecoveryInfoData();
            if (notification.RecoveryInfo != null)
            {
                recoveryInfoData.EstimatedAmount = notification.RecoveryInfo.EstimatedAmount;
                recoveryInfoData.EstimatedUnit = notification.RecoveryInfo.EstimatedUnit;
                recoveryInfoData.CostAmount = notification.RecoveryInfo.CostAmount;
                recoveryInfoData.CostUnit = notification.RecoveryInfo.CostUnit;
                recoveryInfoData.DisposalAmount = notification.RecoveryInfo.DisposalAmount;
                recoveryInfoData.DisposalUnit = notification.RecoveryInfo.DisposalUnit;
            }
            return recoveryInfoData;
        }

        private static RecoveryPercentageData GetRecoveryPercentage(NotificationApplication notification)
        {
            var recoveryPercentageData = new RecoveryPercentageData
            {
                IsProvidedByImporter = notification.IsProvidedByImporter,
                PercentageRecoverable = notification.PercentageRecoverable,
                MethodOfDisposal = notification.MethodOfDisposal ?? string.Empty
            };
            return recoveryPercentageData;
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

        private WasteTypeData GetWasteType(NotificationApplication notification)
        {
            WasteTypeData chemicalComposition = null;
            if (notification.WasteType != null)
            {
                chemicalComposition = wasteTypeMapper.Map(notification.WasteType);
            }
            return chemicalComposition;
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

        private static TechnologyEmployedData GetTechnologyEmployed(NotificationApplication notification)
        {
            var technologyEmployed = new TechnologyEmployedData
            {
                Details = notification.TechnologyEmployed != null ? notification.TechnologyEmployed.Details : string.Empty,
                AnnexProvided = notification.TechnologyEmployed != null && notification.TechnologyEmployed.AnnexProvided
            };
            return technologyEmployed;
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

        private static List<OperationCodeData> GetOperationCodes(NotificationApplication notification)
        {
            var operationCodes = new List<OperationCodeData>();
            foreach (var operationInfo in notification.OperationInfos)
            {
                var ocd = new OperationCodeData
                {
                    Code = operationInfo.OperationCode.DisplayName,
                    Value = operationInfo.OperationCode.Value
                };
                operationCodes.Add(ocd);
            }
            return operationCodes;
        }
    }
}

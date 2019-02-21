namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using Core.ComponentRegistration;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using CodeType = Core.WasteCodes.CodeType;
    using NotificationType = Core.Shared.NotificationType;

    [AutoRegister]
    internal class NotificationProgressService : INotificationProgressService
    {
        private readonly IwsContext context;

        public NotificationProgressService(IwsContext context)
        {
            this.context = context;
        }

        public bool IsComplete(Guid notificationId)
        {
            var progress = GetNotificationProgressInfo(notificationId);

            return progress.IsAllComplete;
        }

        public NotificationApplicationCompletionProgress GetNotificationProgressInfo(Guid notificationId)
        {
            var command = context.Database.Connection.CreateCommand();
            command.CommandText = "[Notification].[uspNotificationProgress] @NotificationId";
            command.Parameters.Add(new SqlParameter("@NotificationId", notificationId));

            var progressResult = new NotificationProgressResult();

            try
            {
                context.Database.Connection.Open();
                var reader = command.ExecuteReader();
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;

                progressResult.Notification = objectContext.Translate<NotificationProgressNotificationResult>(reader).Single();
                reader.NextResult();

                progressResult.Producers = objectContext.Translate<NotificationProgressProducersResult>(reader).ToArray();
                reader.NextResult();

                progressResult.Facilities = objectContext.Translate<NotificationProgressFacilitiesResult>(reader).ToArray();
                reader.NextResult();

                progressResult.WasteCodes = objectContext.Translate<NotificationProgressWasteCodesResult>(reader).ToArray();
                reader.NextResult();

                progressResult.CustomsOffices = objectContext.Translate<NotificationProgressCustomsOfficesResult>(reader).ToArray();
            }
            finally
            {
                context.Database.Connection.Close();
            }

            return GetCompletetionProgress(progressResult);
        }

        private NotificationApplicationCompletionProgress GetCompletetionProgress(NotificationProgressResult progressResult)
        {
            var progress = new NotificationApplicationCompletionProgress();

            progress.Id = progressResult.Notification.Id;
            progress.NotificationType = progressResult.Notification.NotificationType;
            progress.CompetentAuthority = progressResult.Notification.CompetentAuthority;
            progress.NotificationNumber = progressResult.Notification.NotificationNumber;

            var organisationsInvolvedComplete = MapProgressForOrganisationsInvolved(progress, progressResult);
            var recoveryOperationComplete = MapProgressForRecoveryOperation(progress, progressResult);
            var transportationComplete = MapProgressForTransportation(progress, progressResult);
            var journeyComplete = MapProgressForJourney(progress, progressResult);
            var amountsAndDatesComplete = MapProgressForAmountsAndDates(progress, progressResult);
            var classifyYourWasteComplete = MapProgressForClassifyYourWaste(progress, progressResult);
            var wasteCodesComplete = MapProgressForWasteCodes(progress, progressResult);
            var wasteRecoveryComplete = MapProgressForWasteRecovery(progress, progressResult);

            progress.IsAllComplete = organisationsInvolvedComplete
                && recoveryOperationComplete
                && transportationComplete
                && journeyComplete
                && amountsAndDatesComplete
                && classifyYourWasteComplete
                && wasteCodesComplete
                && (progress.NotificationType == NotificationType.Disposal || wasteRecoveryComplete);

            return progress;
        }

        private bool MapProgressForOrganisationsInvolved(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasExporter = progressResult.Notification.ExporterId.HasValue;
            progress.HasProducer = progressResult.Producers.Any(p => p.Id.HasValue);
            progress.HasSiteOfExport = progressResult.Producers.Any(p => p.IsSiteOfExport.GetValueOrDefault());
            progress.HasImporter = progressResult.Notification.ImporterId.HasValue;
            progress.HasFacility = progressResult.Facilities.Any(f => f.Id.HasValue);
            progress.HasActualSiteOfTreatment = progressResult.Facilities.Any(f => f.IsActualSiteOfTreatment.GetValueOrDefault());

            return progress.HasExporter
                && progress.HasProducer
                && progress.HasSiteOfExport
                && progress.HasImporter
                && progress.HasFacility
                && progress.HasActualSiteOfTreatment;
        }

        private bool MapProgressForRecoveryOperation(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasPreconsentedInformation = progressResult.Notification.NotificationType == NotificationType.Disposal
                || progressResult.Notification.IsPreconsentedRecoveryFacility.HasValue;
            progress.HasOperationCodes = progressResult.Notification.OperationCodesId.HasValue;
            progress.HasTechnologyEmployed = progressResult.Notification.TechnologyEmployedId.HasValue;
            progress.HasReasonForExport = !string.IsNullOrWhiteSpace(progressResult.Notification.ReasonForExport);

            return progress.HasPreconsentedInformation
                && progress.HasOperationCodes
                && progress.HasTechnologyEmployed
                && progress.HasReasonForExport;
        }

        private bool MapProgressForTransportation(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasCarrier = progressResult.Notification.CarrierId.HasValue;
            progress.HasMeansOfTransport = !string.IsNullOrWhiteSpace(progressResult.Notification.MeansOfTransport);
            progress.HasPackagingInfo = progressResult.Notification.PackagingInfoId.HasValue;
            progress.HasSpecialHandlingRequirements = progressResult.Notification.HasSpecialHandlingRequirements.HasValue;

            return progress.HasCarrier
                && progress.HasMeansOfTransport
                && progress.HasPackagingInfo
                && progress.HasSpecialHandlingRequirements;
        }

        private bool MapProgressForJourney(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasStateOfExport = progressResult.Notification.StateOfExportId.HasValue;
            progress.HasStateOfImport = progressResult.Notification.StateOfImportId.HasValue;
            progress.HasTransitState = progressResult.Notification.TransitStateId.HasValue;

            if (progressResult.CustomsOffices.Any(co => co.ImportIsEuMember.HasValue && co.ExportIsEuMember.HasValue))
            {
                bool importIsEuMember = progressResult.CustomsOffices.First().ImportIsEuMember.GetValueOrDefault();
                bool exportIsEuMember = progressResult.CustomsOffices.First().ExportIsEuMember.GetValueOrDefault();
                bool allTransitStatsAreEu = progressResult.CustomsOffices.All(co => co.TransitIsEuMember.GetValueOrDefault(true));
                bool hasEntryOffice = progressResult.CustomsOffices.First().EntryCustomsOfficeId.HasValue;
                bool hasExitOffice = progressResult.CustomsOffices.First().ExitCustomsOfficeId.HasValue;

                bool allInEu = importIsEuMember && exportIsEuMember && allTransitStatsAreEu;
                bool importOutsideEu = !importIsEuMember && exportIsEuMember;
                bool transitsOutsideEu = importIsEuMember && exportIsEuMember && !allTransitStatsAreEu;

                progress.HasCustomsOffice = allInEu
                    || (importOutsideEu && hasExitOffice)
                    || (transitsOutsideEu && hasExitOffice && hasEntryOffice);
            }
            else
            {
                progress.HasCustomsOffice = false;
            }

            progress.HasCustomsOfficeSelections = progressResult.CustomsOffices.Any() &&
                                                  (progressResult.CustomsOffices.First().IsEntryCustomsOfficeRequired !=
                                                   null &&
                                                   progressResult.CustomsOffices.First().IsExitCustomsOfficeRequired !=
                                                   null);

            return progress.HasStateOfExport
                && progress.HasStateOfImport
                && progress.HasCustomsOfficeSelections;
        }

        private bool MapProgressForAmountsAndDates(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasShipmentInfo = progressResult.Notification.ShipmentInfoId.HasValue;

            return progress.HasShipmentInfo;
        }

        private bool MapProgressForClassifyYourWaste(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasWasteType = progressResult.Notification.WasteTypeId.HasValue
                && (!string.IsNullOrWhiteSpace(progressResult.Notification.OtherWasteTypeDescription)
                    || progressResult.Notification.WasteTypeHasAnnex.GetValueOrDefault()
                    || progressResult.Notification.WasteAdditionalInformationId.HasValue);
            progress.HasWasteGenerationProcess = progressResult.Notification.IsWasteGenerationProcessAttached.GetValueOrDefault()
                || !string.IsNullOrWhiteSpace(progressResult.Notification.WasteGenerationProcess);
            progress.HasPhysicalCharacteristics = progressResult.Notification.PhysicalCharacteristicsId.HasValue;

            return progress.HasWasteType
                && progress.HasWasteGenerationProcess
                && progress.HasPhysicalCharacteristics;
        }

        private bool MapProgressForWasteCodes(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasBaselOecdCode = progressResult.WasteCodes.Any(wc =>
                wc.CodeType == CodeType.Basel
                || wc.CodeType == CodeType.Oecd);
            progress.HasEwcCodes = progressResult.WasteCodes.Any(wc => wc.CodeType == CodeType.Ewc);
            progress.HasYCodes = progressResult.WasteCodes.Any(wc => wc.CodeType == CodeType.Y);
            progress.HasHCodes = progressResult.WasteCodes.Any(wc => wc.CodeType == CodeType.H);
            progress.HasUnClasses = progressResult.WasteCodes.Any(wc => wc.CodeType == CodeType.Un);
            progress.HasUnNumbers = progressResult.WasteCodes.Any(wc => wc.CodeType == CodeType.UnNumber);

            var otherCodes = new[] { CodeType.CustomsCode, CodeType.ImportCode, CodeType.ExportCode, CodeType.OtherCode };
            progress.HasOtherCodes = otherCodes.All(oc =>
                progressResult.WasteCodes.Select(wc => wc.CodeType).Contains(oc));

            return progress.HasBaselOecdCode
                && progress.HasEwcCodes
                && progress.HasYCodes
                && progress.HasHCodes
                && progress.HasUnClasses
                && progress.HasUnNumbers
                && progress.HasOtherCodes;
        }

        private bool MapProgressForWasteRecovery(NotificationApplicationCompletionProgress progress, NotificationProgressResult progressResult)
        {
            progress.HasRecoveryData = progressResult.Notification.IsRecoveryPercentageDataProvidedByImporter.HasValue
                && (progressResult.Notification.IsRecoveryPercentageDataProvidedByImporter.Value
                || (progressResult.Notification.RecoveryInfoId.HasValue 
                    && (progressResult.Notification.PercentageRecoverable == 100 || progressResult.Notification.DisposalInfoId.HasValue)));

            return progress.HasRecoveryData;
        }

        private class NotificationProgressResult
        {
            public NotificationProgressNotificationResult Notification { get; set; }
            public NotificationProgressProducersResult[] Producers { get; set; }
            public NotificationProgressFacilitiesResult[] Facilities { get; set; }
            public NotificationProgressWasteCodesResult[] WasteCodes { get; set; }
            public NotificationProgressCustomsOfficesResult[] CustomsOffices { get; set; }
        }

        private class NotificationProgressNotificationResult
        {
            public Guid Id { get; set; }
            public NotificationType NotificationType { get; set; }
            public UKCompetentAuthority CompetentAuthority { get; set; }
            public string NotificationNumber { get; set; }
            public bool? IsPreconsentedRecoveryFacility { get; set; }
            public string ReasonForExport { get; set; }
            public bool? HasSpecialHandlingRequirements { get; set; }
            public string MeansOfTransport { get; set; }
            public bool? IsRecoveryPercentageDataProvidedByImporter { get; set; }
            public bool? IsWasteGenerationProcessAttached { get; set; }
            public string WasteGenerationProcess { get; set; }
            public Guid? ExporterId { get; set; }
            public Guid? ImporterId { get; set; }
            public Guid? CarrierId { get; set; }
            public Guid? OperationCodesId { get; set; }
            public Guid? TechnologyEmployedId { get; set; }
            public Guid? PackagingInfoId { get; set; }
            public Guid? PhysicalCharacteristicsId { get; set; }
            public Guid? RecoveryInfoId { get; set; }
            public decimal? PercentageRecoverable { get; set; }
            public Guid? DisposalInfoId { get; set; }
            public Guid? ShipmentInfoId { get; set; }
            public Guid? WasteTypeId { get; set; }
            public string OtherWasteTypeDescription { get; set; }
            public bool? WasteTypeHasAnnex { get; set; }
            public Guid? WasteAdditionalInformationId { get; set; }
            public Guid? StateOfExportId { get; set; }
            public Guid? StateOfImportId { get; set; }
            public Guid? TransitStateId { get; set; }
        }

        private class NotificationProgressProducersResult
        {
            public Guid? Id { get; set; }
            public bool? IsSiteOfExport { get; set; }
        }

        private class NotificationProgressFacilitiesResult
        {
            public Guid? Id { get; set; }
            public bool? IsActualSiteOfTreatment { get; set; }
        }

        private class NotificationProgressWasteCodesResult
        {
            public CodeType? CodeType { get; set; }
        }

        private class NotificationProgressCustomsOfficesResult
        {
            public bool? ImportIsEuMember { get; set; }
            public bool? ExportIsEuMember { get; set; }
            public bool? TransitIsEuMember { get; set; }
            public Guid? EntryCustomsOfficeId { get; set; }
            public Guid? ExitCustomsOfficeId { get; set; }
            public bool? IsEntryCustomsOfficeRequired { get; set; }
            public bool? IsExitCustomsOfficeRequired { get; set; }
        }
    }
}
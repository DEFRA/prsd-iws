namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ImportNotificationValidator : AbstractValidator<ImportNotification>
    {
        public ImportNotificationValidator(IValidator<Exporter> exporterValidator,
            IValidator<FacilityCollection> facilitiesValidator,
            IValidator<Importer> importerValidator,
            IValidator<Preconsented> preconsentedValidator,
            IValidator<Producer> producerValidator,
            IValidator<Shipment> shipmentValidator,
            IValidator<StateOfExport> stateOfExportValidator,
            IValidator<StateOfImport> stateOfImportValidator,
            IValidator<TransitStateCollection> transitStatesValidator,
            IValidator<WasteOperation> wasteOperationValidator,
            IValidator<WasteType> wasteTypeValidator)
        {
            RuleFor(x => x.Exporter).SetValidator(exporterValidator);
            RuleFor(x => x.Facilities).SetValidator(facilitiesValidator);
            RuleFor(x => x.Importer).SetValidator(importerValidator);
            RuleFor(x => x.Preconsented).SetValidator(preconsentedValidator);
            RuleFor(x => x.Producer).SetValidator(producerValidator);
            RuleFor(x => x.Shipment).SetValidator(shipmentValidator);
            RuleFor(x => x.StateOfExport).SetValidator(stateOfExportValidator);
            RuleFor(x => x.StateOfImport).SetValidator(stateOfImportValidator);
            RuleFor(x => x.TransitStates).SetValidator(transitStatesValidator);
            RuleFor(x => x.WasteOperation).SetValidator(wasteOperationValidator);
            RuleFor(x => x.WasteType).SetValidator(wasteTypeValidator);
        }
    }
}
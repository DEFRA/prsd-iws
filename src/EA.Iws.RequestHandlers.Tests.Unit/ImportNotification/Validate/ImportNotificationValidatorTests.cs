namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ImportNotificationValidatorTests
    {
        private readonly ImportNotificationValidator validator;

        public ImportNotificationValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = A.Fake<ContactValidator>();
            var exporterValidator = new ExporterValidator(addressValidator, contactValidator);
            var facilityValidator = A.Fake<FacilityValidator>();
            var facilitiesValidator = new FacilityCollectionValidator(facilityValidator);
            var importerValidator = new ImporterValidator(addressValidator, contactValidator);
            var importRepository = A.Fake<Domain.ImportNotification.IImportNotificationRepository>();
            var preconsentedValidator = new PreconsentedValidator(importRepository);
            var producerValidator = new ProducerValidator(addressValidator, contactValidator);
            var draftImportRepository = A.Fake<DataAccess.Draft.IDraftImportNotificationRepository>();
            var shipmentValidator = new ShipmentValidator(draftImportRepository);
            var entryOrExitPointRepository = A.Fake<Domain.TransportRoute.IEntryOrExitPointRepository>();
            var competentAuthorityRepository = A.Fake<Domain.ICompetentAuthorityRepository>();
            var stateOfExportValidator = new StateOfExportValidator(entryOrExitPointRepository, competentAuthorityRepository);
            var stateOfImportValidator = new StateOfImportValidator(countryRepository, competentAuthorityRepository, entryOrExitPointRepository);
            var transitStateValidator = new TransitStateValidator(entryOrExitPointRepository, competentAuthorityRepository);
            var transitStatesValidator = new TransitStateCollectionValidator(transitStateValidator);
            var wasteOperationValidator = new WasteOperationValidator();
            var wasteTypeValidator = new WasteTypeValidator();
            validator = new ImportNotificationValidator(exporterValidator,
                facilitiesValidator,
                importerValidator,
                preconsentedValidator,
                producerValidator,
                shipmentValidator,
                stateOfExportValidator,
                stateOfImportValidator,
                transitStatesValidator,
                wasteOperationValidator,
                wasteTypeValidator);
        }

        [Fact]
        public void ExporterIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Exporter, typeof(ExporterValidator));
        }

        [Fact]
        public void FacilitiesAreValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Facilities, typeof(FacilityCollectionValidator));
        }

        [Fact]
        public void ImporterIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Importer, typeof(ImporterValidator));
        }

        [Fact]
        public void PreconsentedIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Preconsented, typeof(PreconsentedValidator));
        }

        [Fact]
        public void ProducerIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Producer, typeof(ProducerValidator));
        }

        [Fact]
        public void ShipmentIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Shipment, typeof(ShipmentValidator));
        }

        [Fact]
        public void StateOfExportIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.StateOfExport, typeof(StateOfExportValidator));
        }

        [Fact]
        public void StateOfImportIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.StateOfImport, typeof(StateOfImportValidator));
        }

        [Fact]
        public void TransitStatesAreValidated()
        {
            validator.ShouldHaveChildValidator(x => x.TransitStates, typeof(TransitStateCollectionValidator));
        }

        [Fact]
        public void WasteOperationIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.WasteOperation, typeof(WasteOperationValidator));
        }

        [Fact]
        public void WasteTypeIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.WasteType, typeof(WasteTypeValidator));
        }
    }
}
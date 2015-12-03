namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class StateOfImportValidatorTests
    {
        private readonly StateOfImportValidator validator;

        public StateOfImportValidatorTests()
        {
            validator = new StateOfImportValidator();
        }

        [Fact]
        public void ValidStateOfImport_ResultIsValid()
        {
            var stateOfImport = GetValidStateOfImport();

            var result = validator.Validate(stateOfImport);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CompetentAuthorityNull_HasValidationError()
        {
            var stateOfImport = GetValidStateOfImport();
            stateOfImport.CompetentAuthorityId = null;

            validator.ShouldHaveValidationErrorFor(x => x.CompetentAuthorityId, stateOfImport);
        }

        [Fact]
        public void EntryPointNull_HasValidationError()
        {
            var stateOfImport = GetValidStateOfImport();
            stateOfImport.EntryPointId = null;

            validator.ShouldHaveValidationErrorFor(x => x.EntryPointId, stateOfImport);
        }

        private StateOfImport GetValidStateOfImport()
        {
            return new StateOfImport
            {
                CompetentAuthorityId = new Guid("61879A46-8AF2-4AFE-850F-A424080C806C"),
                EntryPointId = new Guid("E085D5F9-94ED-434C-8EF3-3D0DEFB64E1C")
            };
        }
    }
}
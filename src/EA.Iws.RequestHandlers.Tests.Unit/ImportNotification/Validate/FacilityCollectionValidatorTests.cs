namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class FacilityCollectionValidatorTests
    {
        private readonly FacilityCollectionValidator validator;

        public FacilityCollectionValidatorTests()
        {
            var facilityValidator = A.Fake<IValidator<Facility>>();
            validator = new FacilityCollectionValidator(facilityValidator);
        }

        [Fact]
        public void SiteOfTreatmentNotSet_HasValidationError()
        {
            var facilityCollection = new FacilityCollection
            {
                Facilities = new List<Facility>()
                {
                    new Facility
                    {
                        IsActualSite = false
                    },
                    new Facility
                    {
                        IsActualSite = false
                    }
                }
            };

            validator.ShouldHaveValidationErrorFor(x => x.Facilities, facilityCollection);
        }

        [Fact]
        public void SiteOfTreatmentSet_HasNoValidationError()
        {
            var facilityCollection = new FacilityCollection
            {
                Facilities = new List<Facility>()
                {
                    new Facility
                    {
                        IsActualSite = true
                    },
                    new Facility
                    {
                        IsActualSite = false
                    }
                }
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.Facilities, facilityCollection);
        }
    }
}
namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class FacilityCollectionValidatorTests
    {
        private static readonly Guid AnyGuid = new Guid("9E73A13F-9D3E-478C-88E5-615E00EA7FD9");
        private readonly FacilityCollectionValidator validator;

        public FacilityCollectionValidatorTests()
        {
            var facilityValidator = A.Fake<IValidator<Facility>>();
            validator = new FacilityCollectionValidator(facilityValidator);
        }

        //[Fact]
        //public async void SiteOfTreatmentNotSet_HasValidationError()
        //{
        //    var facilityCollection = new FacilityCollection
        //    {
        //        Facilities = new List<Facility>()
        //        {
        //            new Facility(AnyGuid)
        //            {
        //                IsActualSite = false
        //            },
        //            new Facility(AnyGuid)
        //            {
        //                IsActualSite = false
        //            }
        //        }
        //    };

        //    var result = await validator.TestValidateAsync(facilityCollection);
        //    result.ShouldHaveValidationErrorFor(fc => fc.Facilities);
        //}

        [Fact]
        public async void SiteOfTreatmentSet_HasNoValidationError()
        {
            var facilityCollection = new FacilityCollection
            {
                Facilities = new List<Facility>()
                {
                    new Facility(AnyGuid)
                    {
                        IsActualSite = true
                    },
                    new Facility(AnyGuid)
                    {
                        IsActualSite = false
                    }
                }
            };

            var result = await validator.TestValidateAsync(facilityCollection);
            result.ShouldNotHaveValidationErrorFor(fc => fc.Facilities);
        }

        //[Fact]
        //public async void HasNoFacilities_HasValidationError()
        //{
        //    var facilityCollection = new FacilityCollection();
        //    facilityCollection.Facilities = null;

        //    var result = await validator.TestValidateAsync(facilityCollection);
        //    result.ShouldHaveValidationErrorFor(fc => fc.Facilities);
        //}
    }
}
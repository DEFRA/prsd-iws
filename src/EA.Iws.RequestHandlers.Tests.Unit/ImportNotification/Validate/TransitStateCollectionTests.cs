namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation;
    using FluentValidation.Results;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class TransitStateCollectionTests
    {
        private static readonly Guid AnyGuid = new Guid("2B52905F-1BF5-453C-8CA6-0F5C2C23D9C3");
        private readonly TransitStateCollectionValidator validator;
        private readonly TransitStateCollection transitStateCollection = new TransitStateCollection();

        public TransitStateCollectionTests()
        {
            var transitStateValidator = A.Fake<IValidator<TransitState>>();
            A.CallTo(() => transitStateValidator.Validate(A<TransitState>.Ignored)).Returns(new ValidationResult());

            validator = new TransitStateCollectionValidator(transitStateValidator);
        }

        [Fact]
        public void HasNoTransitStatesSelected_NoTransitStatesEntered_Success()
        {
            transitStateCollection.HasNoTransitStates = true;

            Assert.True(validator.Validate(transitStateCollection).IsValid);
        }

        [Fact]
        public void HasNoTransitStates_TransitStatesEntered_Fails()
        {
            transitStateCollection.HasNoTransitStates = true;
            transitStateCollection.TransitStates.Add(new TransitState(AnyGuid));

            validator.ShouldHaveValidationErrorFor(x => x.HasNoTransitStates, transitStateCollection);
        }

        [Fact]
        public void TransitStates_NoTransitStatesInList_Fails()
        {
            validator.ShouldHaveValidationErrorFor(x => x.HasNoTransitStates, transitStateCollection);
        }

        [Fact]
        public void OneTransitState_OrdinalPositionNotOne_Fails()
        {
            transitStateCollection.TransitStates.Add(new TransitState(AnyGuid)
            {
                OrdinalPosition = 7
            });

            validator.ShouldHaveValidationErrorFor(x => x.TransitStates, transitStateCollection);
        }

        [Fact]
        public void OneTransitState_OrdinalPositionOne_Success()
        {
            transitStateCollection.TransitStates.Add(new TransitState(AnyGuid)
            {
                OrdinalPosition = 1
            });

            Assert.True(validator.Validate(transitStateCollection).IsValid);
        }

        [Fact]
        public void HasTransitStates_ConsecutiveOrdinalPositions_Success()
        {
            transitStateCollection.TransitStates.AddRange(new[]
            {
                new TransitState(AnyGuid) { OrdinalPosition = 1 }, 
                new TransitState(AnyGuid) { OrdinalPosition = 2 }, 
                new TransitState(AnyGuid) { OrdinalPosition = 3 } 
            });

            Assert.True(validator.Validate(transitStateCollection).IsValid);
        }

        [Fact]
        public void HasTransitStates_MissingOrdinalPositions_Fails()
        {
            transitStateCollection.TransitStates.AddRange(new[]
            {
                new TransitState(AnyGuid) { OrdinalPosition = 2 },
                new TransitState(AnyGuid) { OrdinalPosition = 1 },
                new TransitState(AnyGuid) { OrdinalPosition = 5 }
            });

            validator.ShouldHaveValidationErrorFor(x => x.TransitStates, transitStateCollection);
        }

        [Fact]
        public void HasTransitStates_UnorderedInput_ReturnsSuccess()
        {
            transitStateCollection.TransitStates.AddRange(new[]
            {
                new TransitState(AnyGuid) { OrdinalPosition = 3 },
                new TransitState(AnyGuid) { OrdinalPosition = 1 },
                new TransitState(AnyGuid) { OrdinalPosition = 2 }
            });

            Assert.True(validator.Validate(transitStateCollection).IsValid);
        }
    }
}

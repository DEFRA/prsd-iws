namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Domain.ImportNotification;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using TestHelpers.Helpers;
    using Xunit;
    using ImportNotification = Domain.ImportNotification.ImportNotification;

    public class PreconsentedValidatorTests
    {
        private readonly PreconsentedValidator validator;
        private readonly Guid importRecoveryNotificationId = new Guid("8DA0E627-8162-4030-B0EF-F8710C56B0AA");
        private readonly Guid importDisposalNotificationId = new Guid("B5214080-6051-4AF7-8D2D-D1336D8D6158");

        public PreconsentedValidatorTests()
        {
            var importNotificationRepo = A.Fake<IImportNotificationRepository>();

            var importRecoveryNotification = CreateImportNotification(NotificationType.Recovery, importRecoveryNotificationId);
            var importDisposalNotification = CreateImportNotification(NotificationType.Disposal, importRecoveryNotificationId);

            A.CallTo(() => importNotificationRepo.GetByImportNotificationId(importRecoveryNotificationId)).Returns(importRecoveryNotification);
            A.CallTo(() => importNotificationRepo.GetByImportNotificationId(importDisposalNotificationId)).Returns(importDisposalNotification);

            validator = new PreconsentedValidator(importNotificationRepo);
        }

        [Fact]
        public void RecoveryNotification_PreconsentedFacilityExistsIsNull_HasValidationError()
        {
            var preconsented = new Preconsented(importRecoveryNotificationId);

            validator.ShouldHaveValidationErrorFor(x => x.AllFacilitiesPreconsented, preconsented);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RecoveryNotification_PreconsentedFacilityExistsIsNotNull_HasNoValidationError(bool input)
        {
            var preconsented = new Preconsented(importRecoveryNotificationId)
            {
                AllFacilitiesPreconsented = input
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.AllFacilitiesPreconsented, preconsented);
        }

        [Fact]
        public void DisposalNotification_PreconsentedFacilityExistsIsNotNull_HasNoValidationError()
        {
            var preconsented = new Preconsented(importDisposalNotificationId);

            validator.ShouldNotHaveValidationErrorFor(x => x.AllFacilitiesPreconsented, preconsented);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DisposalNotification_PreconsentedFacilityExistsIsNotNull_HasValidationError(bool input)
        {
            var preconsented = new Preconsented(importDisposalNotificationId)
            {
                AllFacilitiesPreconsented = input
            };

            validator.ShouldHaveValidationErrorFor(x => x.AllFacilitiesPreconsented, preconsented);
        }
        
        private ImportNotification CreateImportNotification(NotificationType notificationType, Guid importNotificationId)
        {
            var instance = ObjectInstantiator<ImportNotification>.CreateNew();
            EntityHelper.SetEntityId(instance, importNotificationId);
            ObjectInstantiator<ImportNotification>.SetProperty(x => x.NotificationType, notificationType,
                instance);
            return instance;
        }
    }
}
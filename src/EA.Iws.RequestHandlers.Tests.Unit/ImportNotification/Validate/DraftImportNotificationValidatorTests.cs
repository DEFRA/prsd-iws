namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class DraftImportNotificationValidatorTests
    {
        private DraftImportNotificationValidator validator;

        public DraftImportNotificationValidatorTests()
        {
            validator = new DraftImportNotificationValidator();
        }

        [Fact(Skip = "Not implemented")]
        public void ValidObject_EmptyArray()
        {
            var validNotification = new ImportNotification();

            var result = validator.Validate(validNotification);

            Assert.Empty(result);
        }

        [Fact(Skip = "Not implemented")]
        public void InvalidObject_NonEmptyArray()
        {
            var invalidNotification = new ImportNotification();

            var result = validator.Validate(invalidNotification);

            Assert.NotEmpty(result);
        }
    }
}
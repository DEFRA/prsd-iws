namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Xunit;

    public class NotificationWasteGenerationProcessTests
    {
        private readonly NotificationApplication notification;

        public NotificationWasteGenerationProcessTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void WasteGenerationDescriptionAdded()
        {
            notification.AddWasteGenerationProcess("Process description", true);

            Assert.Equal("Process description", notification.WasteGenerationProcess);
        }

        [Fact]
        public void IsWasteGenerationProcessAttachedIsSet()
        {
            notification.AddWasteGenerationProcess("Process description", true);

            Assert.True(notification.IsWasteGenerationProcessAttached);
        }

        [Fact]
        public void DescriptionCanBeNullIfDocumentAttached()
        {
            notification.AddWasteGenerationProcess(null, true);

            Assert.Equal(null, notification.WasteGenerationProcess);
        }

        [Fact]
        public void DescriptionCanBeEmptyIfDocumentAttached()
        {
            notification.AddWasteGenerationProcess(string.Empty, true);

            Assert.Equal(string.Empty, notification.WasteGenerationProcess);
        }

        [Fact]
        public void DescriptionCannotBeNullIfDocumentNotAttached()
        {
            Action addWasteGenerationProcess = () => notification.AddWasteGenerationProcess(null, false);

            Assert.Throws<InvalidOperationException>(addWasteGenerationProcess);
        }

        [Fact]
        public void DescriptionCannotBeEmptyIfDocumentNotAttached()
        {
            Action addWasteGenerationProcess = () => notification.AddWasteGenerationProcess(string.Empty, false);

            Assert.Throws<InvalidOperationException>(addWasteGenerationProcess);
        }
    }
}
namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationWasteCodeTests
    {
        private readonly NotificationApplication notification;

        public NotificationWasteCodeTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void AddOecdCode_WithValidData_WasteCodeAdded()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Oecd);

            notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.Equal(1, notification.WasteCodeInfo.Count());
        }

        [Fact]
        public void AddBaselCode_AsOptionalCode_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Basel);

            Action addWasteCode =
                () =>
                    notification.AddWasteCode(WasteCodeInfo.CreateOptionalWasteCodeInfo(wasteCode, "optional code",
                        "optional description"));

            Assert.Throws<InvalidOperationException>(addWasteCode);
        }

        [Fact]
        public void AddBaselCode_WhenAlreadySet_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Basel);

            var secondWasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Basel);

            notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Action addWasteCode = () => notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(secondWasteCode));

            Assert.Throws<InvalidOperationException>(addWasteCode);
        }

        [Fact]
        public void AddEwcCode_WhithValidData_EwcCodeAdded()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Ewc);

            notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.True(notification.WasteCodeInfo.Any());
        }

        [Fact]
        public void AddBaselCode_WhenSameCodeExists_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Basel);

            notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Action addWasteCode = () => notification.AddWasteCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.Throws<InvalidOperationException>(addWasteCode);
        }

        [Fact]
        public void AddOptionalWasteCode_WithWrongWasteType_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Ewc);

            Action addWasteCode =
                () =>
                    notification.AddWasteCode(WasteCodeInfo.CreateOptionalWasteCodeInfo(wasteCode, "optional code",
                        "optional description"));

            Assert.Throws<InvalidOperationException>(addWasteCode);
        }

        [Fact]
        public void AddOptionalWasteCode_WithCorrectWasteType_WasteCodeAdded()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.OtherCode);

            notification.AddWasteCode(WasteCodeInfo.CreateOptionalWasteCodeInfo(wasteCode, "optional code",
                "optional description"));

            Assert.Equal(1, notification.WasteCodeInfo.Count());
        }

        private static WasteCode GetTestWasteCode(Guid id, CodeType codeType)
        {
            var wasteCode = ObjectInstantiator<WasteCode>.CreateNew();
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, id, wasteCode);
            ObjectInstantiator<WasteCode>.SetProperty(x => x.CodeType, codeType, wasteCode);
            return wasteCode;
        }
    }
}
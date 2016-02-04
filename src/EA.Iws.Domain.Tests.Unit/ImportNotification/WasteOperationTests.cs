namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Core.Shared;
    using Domain.ImportNotification;
    using TestHelpers.Helpers;
    using Xunit;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class WasteOperationTests
    {
        private readonly Guid importNotificationId = new Guid("1FE8238D-BD7A-4D3A-8188-704EFB2F62F4");
        private readonly OperationCodesList validRCodesList;

        public WasteOperationTests()
        {
            var recoveryImportNotification = new ImportNotification(NotificationType.Recovery, CompetentAuthorityEnum.England, "FR0001");
            EntityHelper.SetEntityId(recoveryImportNotification, importNotificationId);

            validRCodesList = OperationCodesList.CreateForNotification(recoveryImportNotification, new[] { OperationCode.R1, OperationCode.R2 });
        }

        [Fact]
        public void CanCreateOperationCodes()
        {
            var operationCodes = new WasteOperation(importNotificationId, validRCodesList);

            Assert.IsType<WasteOperation>(operationCodes);
        }

        [Fact]
        public void OperationCodesCantBeNull()
        {
            Action createOperationCodes = () => new WasteOperation(importNotificationId, null);

            Assert.Throws<ArgumentNullException>("operationCodes", createOperationCodes);
        }

        [Fact]
        public void ImportNotificationCantBeEmpty()
        {
            Action createOperationCodes = () => new WasteOperation(Guid.Empty, validRCodesList);

            Assert.Throws<ArgumentException>("importNotificationId", createOperationCodes);
        }

        [Fact]
        public void CanSetTechnologyEmployed()
        {
            var operationCodes = new WasteOperation(importNotificationId, validRCodesList);

            operationCodes.SetTechnologyEmployed("techno");

            Assert.Equal("techno", operationCodes.TechnologyEmployed);
        }

        [Fact]
        public void TechnologyEmployedCantBeNull()
        {
            var operationCodes = new WasteOperation(importNotificationId, validRCodesList);

            Action setTechnologyEmployed = () => operationCodes.SetTechnologyEmployed(null);

            Assert.Throws<ArgumentNullException>("technologyEmployed", setTechnologyEmployed);
        }

        [Fact]
        public void TechnologyEmployedCantBeEmpty()
        { 
            var operationCodes = new WasteOperation(importNotificationId, validRCodesList);

            Action setTechnologyEmployed = () => operationCodes.SetTechnologyEmployed(string.Empty);

            Assert.Throws<ArgumentException>("technologyEmployed", setTechnologyEmployed);
        }
    }
}
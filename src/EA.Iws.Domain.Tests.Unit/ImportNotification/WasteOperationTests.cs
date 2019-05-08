namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.Shared;
    using Domain.ImportNotification;
    using TestHelpers.Helpers;
    using Xunit;

    public class WasteOperationTests
    {
        private readonly Guid importNotificationId = new Guid("1FE8238D-BD7A-4D3A-8188-704EFB2F62F4");
        private readonly OperationCodesList validRCodesList;
        private readonly OperationCodesList anotherValidRCodesList;

        public WasteOperationTests()
        {
            var recoveryImportNotification = new ImportNotification(NotificationType.Recovery, UKCompetentAuthority.England, "FR0001");
            EntityHelper.SetEntityId(recoveryImportNotification, importNotificationId);

            validRCodesList = OperationCodesList.CreateForNotification(recoveryImportNotification, new[] { OperationCode.R1, OperationCode.R2 });
            anotherValidRCodesList = OperationCodesList.CreateForNotification(recoveryImportNotification, new[] { OperationCode.R10, OperationCode.R13 });
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
        public void CanSetOperationCodes()
        {
            var operationCodes = new WasteOperation(importNotificationId, validRCodesList);

            operationCodes.SetOperationCodes(new List<WasteOperationCode>(anotherValidRCodesList));

            Assert.All(operationCodes.Codes,
                code => Assert.True(anotherValidRCodesList.Any(x => x.OperationCode == code.OperationCode)));
        }

        [Fact]
        public void SetOperationCodesCantBeNull()
        {
            var operationCodes = new WasteOperation(importNotificationId, validRCodesList);

            Action setOperationCodes = () => operationCodes.SetOperationCodes(null);

            Assert.Throws<ArgumentNullException>("operationCodes", setOperationCodes);
        }
    }
}
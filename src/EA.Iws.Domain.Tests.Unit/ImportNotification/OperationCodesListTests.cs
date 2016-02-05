namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Core.OperationCodes;
    using Core.Shared;
    using Domain.ImportNotification;
    using Xunit;

    public class OperationCodesListTests
    {
        private readonly ImportNotification disposalImportNotification;
        private readonly ImportNotification recoveryImportNotification;

        public OperationCodesListTests()
        {
            recoveryImportNotification = new ImportNotification(NotificationType.Recovery, UKCompetentAuthority.England,
                "FR0001");
            disposalImportNotification = new ImportNotification(NotificationType.Disposal, UKCompetentAuthority.England,
                "FR0001");
        }

        [Fact]
        public void CanCreateOperationCodesList()
        {
            var operationCodes = OperationCodesList.CreateForNotification(recoveryImportNotification,
                new[] { OperationCode.R1, OperationCode.R2 });

            Assert.IsType<OperationCodesList>(operationCodes);
        }

        [Fact]
        public void CantHaveDisposalCodesOnRecoveryNotification()
        {
            Action createOperationCodes =
                () =>
                    OperationCodesList.CreateForNotification(recoveryImportNotification,
                        new[]
                        {
                            OperationCode.D1, OperationCode.D2
                        });

            Assert.Throws<ArgumentException>("operationCodes", createOperationCodes);
        }

        [Fact]
        public void CantHaveRecoveryCodesOnDisposalNotification()
        {
            Action createOperationCodes =
                () =>
                    OperationCodesList.CreateForNotification(disposalImportNotification,
                        new[]
                        {
                            OperationCode.R1, OperationCode.R2
                        });

            Assert.Throws<ArgumentException>("operationCodes", createOperationCodes);
        }

        [Fact]
        public void OperationCodesMustBeUnique()
        {
            Action createOperationCodes =
                () =>
                    OperationCodesList.CreateForNotification(recoveryImportNotification,
                        new[]
                        {
                            OperationCode.R1, OperationCode.R1
                        });

            Assert.Throws<ArgumentException>("operationCodes", createOperationCodes);
        }

        [Fact]
        public void OperationCodesCantBeEmpty()
        {
            Action createOperationCodes =
                () => OperationCodesList.CreateForNotification(recoveryImportNotification, new OperationCode[] { });

            Assert.Throws<ArgumentException>("operationCodes", createOperationCodes);
        }
    }
}
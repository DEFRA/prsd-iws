namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System.Collections.Generic;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class OperationViewModelTests
    {
        private readonly List<OperationInfo> operationInfos;
        private readonly TestableTechnologyEmployed technologyEmployed;
        private readonly TestableNotificationApplication notification;
        private readonly OperationInfoFormatter formatter = new OperationInfoFormatter();

        public OperationViewModelTests()
        {
            operationInfos = new List<OperationInfo>
            {
                new TestableOperationInfo { OperationCode = OperationCode.D1 }
            };

            technologyEmployed = new TestableTechnologyEmployed
            {
                AnnexProvided = true,
                Details = "Do stuff",
                FurtherDetails = "Do more stuff"
            };

            notification = new TestableNotificationApplication
            {
                ReasonForExport = "Washing",
                OperationInfos = operationInfos,
                TechnologyEmployed = technologyEmployed
            };
        }

        [Fact]
        public void SetsAllFieldsToEmptyStringOrFalseWhereNotificationIsNull()
        {
            var model = new OperationViewModel(null, formatter);

            Assert.Equal(string.Empty, model.AnnexProvided);
            Assert.Equal(string.Empty, model.FurtherDetails);
            Assert.Equal(string.Empty, model.TechnologyEmployedDetails);
            Assert.Equal(string.Empty, model.ReasonForExport);
            Assert.Equal(string.Empty, model.OperationCodes);
            Assert.False(model.IsAnnexProvided);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetsAnnexProvided(bool isAnnexProvided)
        {
            technologyEmployed.AnnexProvided = isAnnexProvided;

            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(isAnnexProvided, model.IsAnnexProvided);
        }

        [Fact]
        public void SetsReasonForExportWhereGiven()
        {
            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(notification.ReasonForExport, model.ReasonForExport);
        }

        [Fact]
        public void SetsReasonForExportWhereNullToEmptyString()
        {
            notification.ReasonForExport = null;

            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(string.Empty, model.ReasonForExport);
        }

        [Fact]
        public void SetsTechnologyEmployedDetailsWhereGiven()
        {
            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(technologyEmployed.Details, model.TechnologyEmployedDetails);
        }

        [Fact]
        public void SetsTechnologyEmployedDetailsWhereGivenAsNullToEmptyString()
        {
            technologyEmployed.Details = null;

            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(string.Empty, model.TechnologyEmployedDetails);
        }
        
        [Fact]
        public void SetsTechnologyEmployedDetailsEmptyStringWhereTechnologyEmployedIsNull()
        {
            notification.TechnologyEmployed = null;

            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(string.Empty, model.TechnologyEmployedDetails);
        }

        [Fact]
        public void SetsFurtherDetailsWhereGiven()
        {
            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(technologyEmployed.FurtherDetails, model.FurtherDetails);
        }

        [Fact]
        public void SetsFurtherDetailsWhereGivenAsNullToEmptyString()
        {
            technologyEmployed.FurtherDetails = null;

            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(string.Empty, model.FurtherDetails);
        }

        [Fact]
        public void SetsFurtherDetailsEmptyStringWhereTechnologyEmployedIsNull()
        {
            notification.TechnologyEmployed = null;

            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(string.Empty, model.FurtherDetails);
        }

        [Fact]
        public void SetsAnnexProvidedToEmptyString()
        {
            var model = new OperationViewModel(notification, formatter);

            Assert.Equal(string.Empty, model.AnnexProvided);
        }
    }
}

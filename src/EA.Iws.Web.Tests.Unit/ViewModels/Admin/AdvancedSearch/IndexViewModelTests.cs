namespace EA.Iws.Web.Tests.Unit.ViewModels.Admin.AdvancedSearch
{
    using Areas.Admin.ViewModels.AdvancedSearch;
    using Core.ImportNotificationAssessment;
    using Core.NotificationAssessment;
    using Xunit;

    public class IndexViewModelTests
    {
        private readonly IndexViewModel model;

        public IndexViewModelTests()
        {
            model = new IndexViewModel();
        }

        [Theory]
        [InlineData(ImportNotificationStatus.FileClosed)]
        [InlineData(ImportNotificationStatus.AwaitingAssessment)]
        [InlineData(ImportNotificationStatus.AwaitingPayment)]
        [InlineData(ImportNotificationStatus.ConsentWithdrawn)]
        [InlineData(ImportNotificationStatus.Consented)]
        [InlineData(ImportNotificationStatus.DecisionRequiredBy)]
        [InlineData(ImportNotificationStatus.InAssessment)]
        [InlineData(ImportNotificationStatus.NotificationReceived)]
        [InlineData(ImportNotificationStatus.Objected)]
        [InlineData(ImportNotificationStatus.ReadyToAcknowledge)]
        [InlineData(ImportNotificationStatus.Submitted)]
        [InlineData(ImportNotificationStatus.Withdrawn)]
        public void CanParseImportStatus(ImportNotificationStatus status)
        {
            model.SelectedNotificationStatusId = (int)status + 500;

            Assert.Equal(status, model.GetImportNotificationStatus());
        }

        [Fact]
        public void CanParseNullImportStatus()
        {
            model.SelectedNotificationStatusId = null;

            Assert.Equal(null, model.GetImportNotificationStatus());
        }

        [Theory]
        [InlineData(NotificationStatus.FileClosed)]
        [InlineData(NotificationStatus.ConsentWithdrawn)]
        [InlineData(NotificationStatus.Consented)]
        [InlineData(NotificationStatus.DecisionRequiredBy)]
        [InlineData(NotificationStatus.InAssessment)]
        [InlineData(NotificationStatus.NotificationReceived)]
        [InlineData(NotificationStatus.Objected)]
        [InlineData(NotificationStatus.ReadyToTransmit)]
        [InlineData(NotificationStatus.Reassessment)]
        [InlineData(NotificationStatus.Submitted)]
        [InlineData(NotificationStatus.Transmitted)]
        [InlineData(NotificationStatus.Unlocked)]
        [InlineData(NotificationStatus.Withdrawn)]
        public void CanParseExportStatus(NotificationStatus status)
        {
            model.SelectedNotificationStatusId = (int)status;

            Assert.Equal(status, model.GetExportNotificationStatus());
        }

        [Fact]
        public void CanParseNullExportStatus()
        {
            model.SelectedNotificationStatusId = null;

            Assert.Equal(null, model.GetExportNotificationStatus());
        }
    }
}
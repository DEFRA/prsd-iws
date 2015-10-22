namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System.Linq;
    using Core.NotificationAssessment;
    using Web.ViewModels.Applicant;
    using Xunit;

    public class ApprovedNotificationViewModelTests
    {
        private readonly NotificationAssessmentSummaryInformationData notification;

        public ApprovedNotificationViewModelTests()
        {
            notification = new NotificationAssessmentSummaryInformationData();
        }

        [Fact]
        public void NotificationStatus_Submitted_Shows_ThreeUserChoices()
        {
            notification.Status = NotificationStatus.Submitted;
            var approvedNotificationViewModel = new ApprovedNotificationViewModel(notification);
            Assert.NotNull(approvedNotificationViewModel);
            Assert.Equal(3, approvedNotificationViewModel.UserChoices.PossibleValues.Count);
        }

        [Fact]
        public void NotificationStatus_Submitted_Shows_AllUserChoices()
        {
            notification.Status = NotificationStatus.Consented;
            var approvedNotificationViewModel = new ApprovedNotificationViewModel(notification);
            Assert.NotNull(approvedNotificationViewModel);
            Assert.Equal(4, approvedNotificationViewModel.UserChoices.PossibleValues.Count);
        }

        [Fact]
        public void NotificationStatus_NotSubmitted_Shows_AllUserChoices()
        {
            notification.Status = NotificationStatus.NotSubmitted;
            var approvedNotificationViewModel = new ApprovedNotificationViewModel(notification);
            Assert.NotNull(approvedNotificationViewModel);
            Assert.Equal(2, approvedNotificationViewModel.UserChoices.PossibleValues.Count);
        }

        [Fact]
        public void NotificationStatus_NotSubmitted_UserChoices_ShouldNotContain_PrintDocument()
        {
            notification.Status = NotificationStatus.NotSubmitted;
            var approvedNotificationViewModel = new ApprovedNotificationViewModel(notification);
            Assert.NotNull(approvedNotificationViewModel);
            Assert.False(approvedNotificationViewModel.UserChoices.PossibleValues.Any(x => x.Key == "Print notification document"));
        }

        [Fact]
        public void NotificationStatus_NotSubmitted_UserChoices_ShouldNotContain_ManageShipments()
        {
            notification.Status = NotificationStatus.NotSubmitted;
            var approvedNotificationViewModel = new ApprovedNotificationViewModel(notification);
            Assert.NotNull(approvedNotificationViewModel);
            Assert.False(approvedNotificationViewModel.UserChoices.PossibleValues.Any(x => x.Key == "Manage shipments"));
        }
    }
}

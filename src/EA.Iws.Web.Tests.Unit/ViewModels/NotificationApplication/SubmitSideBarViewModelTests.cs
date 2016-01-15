namespace EA.Iws.Web.Tests.Unit.ViewModels.NotificationApplication
{
    using Areas.NotificationApplication.ViewModels.NotificationApplication;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Xunit;

    public class SubmitSideBarViewModelTests
    {
        private SubmitSideBarViewModel CreateModel(bool isComplete, NotificationStatus status)
        {
            var submitSummaryData = new SubmitSummaryData
            {
                Status = status
            };

            var progress = new NotificationApplicationCompletionProgress
            {
                IsAllComplete = isComplete
            };

            return new SubmitSideBarViewModel(submitSummaryData, 500, progress);
        }

        [Fact]
        public void ShowSubmitButton_NotificationCompleteAndNotSubmitted_True()
        {
            var model = CreateModel(isComplete: true, status: NotificationStatus.NotSubmitted);

            Assert.True(model.ShowSubmitButton);
        }

        [Fact]
        public void ShowSubmitButton_NotificationIncompleteAndNotSubmitted_False()
        {
            var model = CreateModel(isComplete: false, status: NotificationStatus.NotSubmitted);

            Assert.False(model.ShowSubmitButton);
        }

        [Fact]
        public void ShowSubmitButton_NotificationCompleteAndSubmitted_False()
        {
            var model = CreateModel(isComplete: true, status: NotificationStatus.Submitted);

            Assert.False(model.ShowSubmitButton);
        }

        [Fact]
        public void ShowDisabledSubmitButtonAndGuidanceText_NotificationIncompleteAndNotSubmitted_True()
        {
            var model = CreateModel(isComplete: false, status: NotificationStatus.NotSubmitted);

            Assert.True(model.ShowDisabledSubmitButtonAndGuidanceText);
        }

        [Fact]
        public void ShowDisabledSubmitButtonAndGuidanceText_NotificationCompleteAndNotSubmitted_False()
        {
            var model = CreateModel(isComplete: true, status: NotificationStatus.NotSubmitted);

            Assert.False(model.ShowDisabledSubmitButtonAndGuidanceText);
        }

        [Fact]
        public void ShowDisabledSubmitButtonAndGuidanceText_NotificationCompleteAndSubmitted_False()
        {
            var model = CreateModel(isComplete: true, status: NotificationStatus.Submitted);

            Assert.False(model.ShowDisabledSubmitButtonAndGuidanceText);
        }

        [Fact]
        public void ShowResubmitButton_NotificationNotUnlocked_False()
        {
            var model = CreateModel(isComplete: true, status: NotificationStatus.Submitted);

            Assert.False(model.ShowResubmitButton);
        }

        [Fact]
        public void ShowResubmitButton_NotificationUnlocked_True()
        {
            var model = CreateModel(isComplete: true, status: NotificationStatus.Unlocked);

            Assert.True(model.ShowResubmitButton);
        }
    }
}
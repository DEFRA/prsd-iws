namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using Web.Controllers;
    using Web.ViewModels.Applicant;
    using Xunit;

    public class ApplicantControllerTests
    {
        private readonly ApplicantController applicantController;
        private readonly IMediator mediator;
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly NotificationAssessmentSummaryInformationData notification; 

        public ApplicantControllerTests()
        {
            mediator = A.Fake<IMediator>();
            applicantController = new ApplicantController(mediator);
            notification = new NotificationAssessmentSummaryInformationData();
        }

        [Fact]
        public async Task UserSelects_PrintNotification_RedirectsTo_GenerateNotificationDocument()
        {
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.PrintNotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("GenerateNotificationDocument", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
            Assert.Equal("NotificationApplication", result.RouteValues["area"]);
        }

        [Fact]
        public async Task UserSelects_ViewNotification_Status_Not_NotSubmitted_RedirectsTo_Index()
        {
            A.CallTo(() =>
                mediator.SendAsync(A<GetNotificationAssessmentSummaryInformation>.Ignored))
                .Returns(new NotificationAssessmentSummaryInformationData() { Status = NotificationStatus.Submitted });
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.ViewNotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
            Assert.Equal("NotificationApplication", result.RouteValues["area"]);
        }

        [Fact]
        public async Task UserSelects_ViewNotification_Status_NotSubmitted_RedirectsTo_Exporter()
        {
            A.CallTo(() =>
                mediator.SendAsync(A<GetNotificationAssessmentSummaryInformation>.Ignored))
                .Returns(new NotificationAssessmentSummaryInformationData() { Status = NotificationStatus.NotSubmitted });
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.ViewNotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Exporter", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
            Assert.Equal("NotificationApplication", result.RouteValues["area"]);
        }
        
        private ApprovedNotificationViewModel GetApprovedNotificationViewModel(UserChoice userChoice)
        {
            notification.Status = NotificationStatus.Submitted;
            var approvedNotificationViewModel = new ApprovedNotificationViewModel(notification);
            approvedNotificationViewModel.NotificationId = notificationId;
            approvedNotificationViewModel.UserChoices.SelectedValue = (int)userChoice;
            return approvedNotificationViewModel;
        }

        private enum UserChoice
        {
            ViewNotification = 1,
            PrintNotification = 2,
            GeneratePrenotification = 3,
            RecordCertificateOfReceipt = 4,
            RecordCertificateOfDisposal = 5,
            RecordCertificateOfRecovery = 6
        }
    }
}

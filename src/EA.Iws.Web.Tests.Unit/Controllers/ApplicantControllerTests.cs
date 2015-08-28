namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.NotificationAssessment;
    using Core.Shared;
    using FakeItEasy;
    using Requests.NotificationAssessment;
    using Web.Controllers;
    using Web.ViewModels.Applicant;
    using Xunit;

    public class ApplicantControllerTests
    {
        private readonly ApplicantController applicantController;
        private readonly IIwsClient client;
        private readonly Guid notificationId = Guid.NewGuid();

        public ApplicantControllerTests()
        {
            client = A.Fake<IIwsClient>();
            applicantController = new ApplicantController(() => client);
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
                client.SendAsync(A<string>.Ignored, A<GetNotificationAssessmentSummaryInformation>.Ignored))
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
                client.SendAsync(A<string>.Ignored, A<GetNotificationAssessmentSummaryInformation>.Ignored))
                .Returns(new NotificationAssessmentSummaryInformationData() { Status = NotificationStatus.NotSubmitted });
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.ViewNotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Exporter", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
            Assert.Equal("NotificationApplication", result.RouteValues["area"]);
        }

        [Fact]
        public async Task UserSelects_GeneratePrenotification_RedirectsTo_MovementsIndex()
        {
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.GeneratePrenotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["notificationId"]);
        }

        [Fact]
        public async Task UserSelects_RecordCertificateOfReceipt_RedirectsTo_MyHome()
        {
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.RecordCertificateOfReceipt)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Home", result.RouteValues["action"]);
            Assert.Equal("Applicant", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        [Fact]
        public async Task UserSelects_RecordCertificateOfDisposal_RedirectsTo_MyHome()
        {
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.RecordCertificateOfDisposal)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Home", result.RouteValues["action"]);
            Assert.Equal("Applicant", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        [Fact]
        public async Task UserSelects_RecordCertificateOfRecovery_RedirectsTo_MyHome()
        {
            var result = await applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.RecordCertificateOfRecovery)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Home", result.RouteValues["action"]);
            Assert.Equal("Applicant", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        private ApprovedNotificationViewModel GetApprovedNotificationViewModel(UserChoice userChoice)
        {
            var approvedNotificationViewModel = new ApprovedNotificationViewModel(NotificationType.Recovery);
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

namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Shared;
    using FakeItEasy;
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
        public void UserSelects_PrintNotification_RedirectsTo_GenerateNotificationDocument()
        {
            var result = applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.PrintNotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("GenerateNotificationDocument", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
            Assert.Equal("NotificationApplication", result.RouteValues["area"]);
        }

        [Fact]
        public void UserSelects_ViewNotification_RedirectsTo_Index()
        {
            var result = applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.ViewNotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
            Assert.Equal("NotificationApplication", result.RouteValues["area"]);
        }

        [Fact]
        public void UserSelects_GeneratePrenotification_RedirectsTo_Actual_Shipment_Date()
        {
            var result = applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.GeneratePrenotification)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Home", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        [Fact]
        public void UserSelects_RecordCertificateOfReceipt_RedirectsTo_MyHome()
        {
            var result = applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.RecordCertificateOfReceipt)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Home", result.RouteValues["action"]);
            Assert.Equal("Applicant", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        [Fact]
        public void UserSelects_RecordCertificateOfDisposal_RedirectsTo_MyHome()
        {
            var result = applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.RecordCertificateOfDisposal)) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Home", result.RouteValues["action"]);
            Assert.Equal("Applicant", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        [Fact]
        public void UserSelects_RecordCertificateOfRecovery_RedirectsTo_MyHome()
        {
            var result = applicantController.ApprovedNotification(GetApprovedNotificationViewModel(UserChoice.RecordCertificateOfRecovery)) as RedirectToRouteResult;
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

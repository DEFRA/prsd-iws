namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.AdminExportAssessment.Controllers;
    using Areas.AdminExportAssessment.ViewModels.Decision;
    using Core.Admin;
    using Core.NotificationAssessment;
    using FakeItEasy;
    using Mappings.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using Web.ViewModels.Shared;
    using Xunit;

    public class DecisionControllerTests
    {
        private readonly IMediator mediator;
        private readonly DecisionController decisionController;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private static readonly DateTime Today = DateTime.UtcNow;
        private readonly DateTime acknowledgedOnDate = Today.AddMonths(-1);

        public DecisionControllerTests()
        {
            mediator = A.Fake<IMediator>();

            A.CallTo(() => mediator.SendAsync(A<GetNotificationAssessmentDecisionData>.Ignored))
                .Returns(new NotificationAssessmentDecisionData
                {
                    AcknowledgedOnDate = acknowledgedOnDate
                });

            decisionController = new DecisionController(mediator, new NotificationAssessmentDecisionDataMap());
        }

        [Fact]
        public async Task InvalidModel_ReturnsView()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.DecisionTypes.Insert(0, DecisionType.Consent);
            decisionController.ModelState.AddModelError("Test", "Error");

            var result = await decisionController.Index(notificationId, model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task ValidConsentData_Posts()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;

            await decisionController.Index(notificationId, model);

            A.CallTo(() => mediator.SendAsync(A<ConsentNotificationApplication>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task DecisionMadeDate_Today_Valid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentedDate = new OptionalDateInputViewModel(Today);

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task DecisionMadeDate_InPastAfterAcknowledgedOnDate_Valid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentedDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task DecisionMadeDate_InFuture_Invalid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentedDate = new OptionalDateInputViewModel(Today.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentedDate"));
        }

        [Fact]
        public async Task DecisionMadeDate_BeforeAcknowledgedOnDate_Invalid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentedDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(-1));
            
            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentedDate"));
        }

        [Fact]
        public async Task ValidFromDate_Today_Valid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidFromDate_InPastAfterAcknowledgedOnDate_Valid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidFromDate_InFuture_Valid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidFromDate_BeforeAcknowledgedOnDate_Invalid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(-1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidFromDate"));
        }

        [Fact]
        public async Task ValidToDate_Preconsented_GreaterThanThreeYearsFromValidFromDate_Invalid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(3).AddDays(1));

            A.CallTo(() => mediator.SendAsync(A<GetNotificationAssessmentDecisionData>.Ignored))
                .Returns(new NotificationAssessmentDecisionData
                {
                    AcknowledgedOnDate = acknowledgedOnDate,
                    IsPreconsented = true
                });

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidToDate"));
        }

        [Fact]
        public async Task ValidToDate_Preconsented_UpToThreeYearsFromValidFromDate_Valid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(3));

            A.CallTo(() => mediator.SendAsync(A<GetNotificationAssessmentDecisionData>.Ignored))
                .Returns(new NotificationAssessmentDecisionData
                {
                    AcknowledgedOnDate = acknowledgedOnDate,
                    IsPreconsented = true
                });

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidToDate_NotPreconsented_GreaterThanOneYearFromValidFromDate_Invalid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(1).AddDays(1));

            A.CallTo(() => mediator.SendAsync(A<GetNotificationAssessmentDecisionData>.Ignored))
                .Returns(new NotificationAssessmentDecisionData
                {
                    AcknowledgedOnDate = acknowledgedOnDate,
                    IsPreconsented = false
                });

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidToDate"));
        }

        [Fact]
        public async Task ValidToDate_NotPreconsented_UpToOneYearFromValidFromDate_Valid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(1));

            A.CallTo(() => mediator.SendAsync(A<GetNotificationAssessmentDecisionData>.Ignored))
                .Returns(new NotificationAssessmentDecisionData
                {
                    AcknowledgedOnDate = acknowledgedOnDate,
                    IsPreconsented = false
                });

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidToDate_Today_Invalid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today);

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidToDate"));
        }

        [Fact]
        public async Task ValidToDate_InPast_Invalid()
        {
            var model = new NotificationAssessmentDecisionViewModel();
            model.SelectedDecision = DecisionType.Consent;
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddDays(-1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidToDate"));
        }
    }
}

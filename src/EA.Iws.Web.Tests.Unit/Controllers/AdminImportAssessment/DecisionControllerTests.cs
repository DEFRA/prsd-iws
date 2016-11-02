namespace EA.Iws.Web.Tests.Unit.Controllers.AdminImportAssessment
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.AdminImportAssessment.Controllers;
    using Areas.AdminImportAssessment.ViewModels.Decision;
    using Core.Admin;
    using Core.ImportNotificationAssessment;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using Web.ViewModels.Shared;
    using Xunit;

    public class DecisionControllerTests
    {
        private readonly IMediator mediator;
        private readonly DecisionController decisionController;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private static readonly DateTime Today = SystemTime.UtcNow;
        private readonly DateTime acknowledgedOnDate = Today.AddMonths(-1);

        public DecisionControllerTests()
        {
            mediator = A.Fake<IMediator>();
            decisionController = new DecisionController(mediator);

            A.CallTo(() => mediator.SendAsync(A<GetImportNotificationAssessmentDecisionData>.Ignored))
                .Returns(new ImportNotificationAssessmentDecisionData
                {
                    AcknowledgedOnDate = acknowledgedOnDate
                });
        }

        [Fact]
        public async Task InvalidModel_ReturnsView()
        {
            var model = new DecisionViewModel();
            model.DecisionTypes.Insert(0, DecisionType.Consent);
            decisionController.ModelState.AddModelError("Test", "Error");

            var result = await decisionController.Index(notificationId, model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task ValidConsentData_Posts()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;

            await decisionController.Index(notificationId, model);

            A.CallTo(() => mediator.SendAsync(A<Consent>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task ConsentGivenDate_Today_Valid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentGivenDate = new OptionalDateInputViewModel(Today);

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ConsentGivenDate_InFuture_Invalid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentGivenDate = new OptionalDateInputViewModel(Today.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentGivenDate"));
        }

        [Fact]
        public async Task ConsentGivenDate_AfterAcknowledgedOnDate_Valid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentGivenDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ConsentGivenDate_BeforeAcknowledgedOnDate_Invalid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentGivenDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(-1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentGivenDate"));
        }

        [Fact]
        public async Task ValidFromDate_Today_Valid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidFromDate_InPastAfterAcknowledgedOnDate_Valid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidFromDate_InFuture_Valid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today.AddDays(1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.IsValid);
        }

        [Fact]
        public async Task ValidFromDate_BeforeAcknowledgedOnDate_Invalid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(acknowledgedOnDate.AddDays(-1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidFromDate"));
        }

        [Fact]
        public async Task ValidToDate_Preconsented_GreaterThanThreeYearsFromValidFromDate_Invalid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(3));

            A.CallTo(() => mediator.SendAsync(A<GetImportNotificationAssessmentDecisionData>.Ignored))
                .Returns(new ImportNotificationAssessmentDecisionData
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
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(3).AddDays(-1));

            A.CallTo(() => mediator.SendAsync(A<GetImportNotificationAssessmentDecisionData>.Ignored))
                .Returns(new ImportNotificationAssessmentDecisionData
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
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(1));

            A.CallTo(() => mediator.SendAsync(A<GetImportNotificationAssessmentDecisionData>.Ignored))
                .Returns(new ImportNotificationAssessmentDecisionData
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
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidFromDate = new OptionalDateInputViewModel(Today);
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddYears(1).AddDays(-1));

            A.CallTo(() => mediator.SendAsync(A<GetImportNotificationAssessmentDecisionData>.Ignored))
                .Returns(new ImportNotificationAssessmentDecisionData
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
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today);

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidToDate"));
        }

        [Fact]
        public async Task ValidToDate_InPast_Invalid()
        {
            var model = new DecisionViewModel();
            model.Decision = DecisionType.Consent;
            model.ConsentValidToDate = new OptionalDateInputViewModel(Today.AddDays(-1));

            await decisionController.Index(notificationId, model);

            Assert.True(decisionController.ModelState.ContainsKey("ConsentValidToDate"));
        }
    }
}

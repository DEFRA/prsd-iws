namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Areas.NotificationMovements.Controllers;
    using Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload;
    using Core.Movement;
    using Core.Rules;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using Web.Infrastructure;
    using Web.Infrastructure.BulkPrenotification;
    using Web.ViewModels.Shared;
    using Xunit;

    public class PrenotificationBulkUploadControllerTests
    {
        private readonly PrenotificationBulkUploadController controller;
        private readonly IMediator mediator;

        public PrenotificationBulkUploadControllerTests()
        {
            mediator = A.Fake<IMediator>();
            var validator = A.Fake<IBulkMovementValidator>();
            var fileReader = A.Fake<IFileReader>();

            controller = new PrenotificationBulkUploadController(this.mediator, validator, fileReader);

            var request = A.Fake<HttpRequestBase>();
            var context = A.Fake<HttpContextBase>();

            A.CallTo(() => request.Url).Returns(new Uri("https://test.com"));
            A.CallTo(() => context.Request).Returns(request);

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
        }

        [Fact]
        public async Task GetIndex_ReturnsView()
        {
            var result = await controller.Index(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
        }

        [Fact]
        public void PostIndex_RedirectsToUpload()
        {
            var model = new PrenotificationBulkUploadViewModel(Guid.NewGuid());

            var result = controller.Index(model.NotificationId, model) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("UploadPrenotifications", (string)result.RouteValues["action"]);
        }

        [Fact]
        public async Task GetUploadPrenotifications_ReturnsView()
        {
            var result = await controller.Index(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
        }

        [Fact]
        public async Task PostUploadPrenotifications_MissingFile_ReturnsView()
        {
            controller.ModelState.AddModelError("File", "Missing file");

            var model = new PrenotificationBulkUploadViewModel(Guid.NewGuid());

            var result = await controller.UploadPrenotifications(model.NotificationId, model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void GetWarning_ReturnsView()
        {
            var result = controller.Warning(Guid.NewGuid()) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void PostWarning_LeaveUpload_RedirectsToOptions()
        {
            var model = new WarningChoiceViewModel(Guid.NewGuid());

            var warningChoice = RadioButtonStringCollectionViewModel.CreateFromEnum(WarningChoicesList.Leave);

            model.WarningChoices = warningChoice;

            var result = controller.Warning(model, string.Empty) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", (string)result.RouteValues["action"]);
            Assert.Equal("Options", (string)result.RouteValues["controller"]);
        }

        private List<RuleResult<MovementRules>> CreateRule(MovementRules rule)
        {
            List<RuleResult<MovementRules>> rules = new List<RuleResult<MovementRules>>();

            RuleResult<MovementRules> ruleResult = new RuleResult<MovementRules>(rule, MessageLevel.Error);
            rules.Add(ruleResult);

            return rules;
        }

        [Fact]
        public async Task Index_Send_Request()
        {
            MovementRulesSummary movementRulesSummary = new MovementRulesSummary(new List<RuleResult<MovementRules>>());
            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored)).Returns(movementRulesSummary);

            var result = await controller.Index(Guid.NewGuid()) as ViewResult;

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index");
        }

        [Fact]
        public async Task Index_Send_Request_ConsentWithdrawn()
        {
            MovementRulesSummary movementRulesSummary = new MovementRulesSummary(CreateRule(MovementRules.ConsentWithdrawn));
            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored)).Returns(movementRulesSummary);

            var result = await controller.Index(Guid.NewGuid()) as RedirectToRouteResult;

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            Assert.True(result.RouteValues["action"].ToString() == "ConsentWithdrawn");
        }

        [Fact]
        public async Task Index_Send_Request_ShipmentLimitReached()
        {
            MovementRulesSummary movementRulesSummary = new MovementRulesSummary(CreateRule(MovementRules.TotalShipmentsReached));
            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored)).Returns(movementRulesSummary);

            var result = await controller.Index(Guid.NewGuid()) as RedirectToRouteResult;

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            Assert.True(result.RouteValues["action"].ToString() == "TotalMovementsReached");
        }

        [Fact]
        public async Task Index_Send_Request_QuantityReached()
        {
            MovementRulesSummary movementRulesSummary = new MovementRulesSummary(CreateRule(MovementRules.TotalIntendedQuantityReached));
            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored)).Returns(movementRulesSummary);

            var result = await controller.Index(Guid.NewGuid()) as RedirectToRouteResult;

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            Assert.True(result.RouteValues["action"].ToString() == "TotalIntendedQuantityReached");
        }

        [Fact]
        public async Task Index_Send_Request_QuantityExceeded()
        {
            MovementRulesSummary movementRulesSummary = new MovementRulesSummary(CreateRule(MovementRules.TotalIntendedQuantityExceeded));
            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored)).Returns(movementRulesSummary);

            var result = await controller.Index(Guid.NewGuid()) as RedirectToRouteResult;

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            Assert.True(result.RouteValues["action"].ToString() == "TotalIntendedQuantityExceeded");
        }

        [Fact]
        public async Task Index_Send_Request_ConsentExpired()
        {
            MovementRulesSummary movementRulesSummary = new MovementRulesSummary(CreateRule(MovementRules.ConsentPeriodExpired));
            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored)).Returns(movementRulesSummary);

            var result = await controller.Index(Guid.NewGuid()) as RedirectToRouteResult;

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            Assert.True(result.RouteValues["action"].ToString() == "ConsentPeriodExpired");
        }
    }
}

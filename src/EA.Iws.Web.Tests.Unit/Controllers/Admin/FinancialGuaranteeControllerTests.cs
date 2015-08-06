namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Admin.Controllers;
    using Areas.Admin.ViewModels;
    using Areas.Admin.ViewModels.FinancialGuarantee;
    using Core.Admin;
    using Core.FinancialGuarantee;
    using FakeItEasy;
    using Mappings;
    using Requests.Admin.FinancialGuarantee;
    using TestHelpers;
    using Xunit;

    public class FinancialGuaranteeControllerTests
    {
        private static readonly Guid AnyGuid = new Guid("3E2AFB2D-03C8-4EF0-878A-FC90A63B32A6");
        private static readonly DateTime AnyDate = new DateTime(2015, 5, 5);

        private static readonly Guid ReceivedId = new Guid("F3B16647-D764-4F0E-B37D-A1F0F02999A4");
        private static readonly FinancialGuaranteeData ReceivedFinancialGuaranteeData = new FinancialGuaranteeData
                {
                    CompletedDate = null,
                    DecisionRequiredDate = null,
                    ReceivedDate = AnyDate,
                    Status = FinancialGuaranteeStatus.ApplicationReceived
                };

        private static readonly Guid CompletedId = new Guid("2E7AC78D-29DE-47AD-BD03-6791F0578416");
        private static readonly FinancialGuaranteeData CompletedFinancialGuaranteeData = new FinancialGuaranteeData
        {
            CompletedDate = AnyDate.AddDays(3),
            ReceivedDate = AnyDate,
            DecisionRequiredDate = AnyDate.AddDays(20),
            Status = FinancialGuaranteeStatus.ApplicationComplete
        };

        private static readonly Guid ApprovedId = new Guid("81EB84DA-5147-45A5-B5AF-A7E5ED2FB942");
        private static readonly FinancialGuaranteeData ApprovedFinancialGuaranteeData = new FinancialGuaranteeData
        {
            CompletedDate = AnyDate.AddDays(3),
            ReceivedDate = AnyDate,
            DecisionRequiredDate = AnyDate.AddDays(20),
            Status = FinancialGuaranteeStatus.Approved,
            Decision = FinancialGuaranteeDecision.Approved,
            DecisionDate = AnyDate.AddDays(5)
        };

        private readonly FinancialGuaranteeController controller;
        private readonly IIwsClient client;
        private readonly FinancialGuaranteeDatesViewModel model;

        public FinancialGuaranteeControllerTests()
        {
            client = A.Fake<IIwsClient>();

            controller = new FinancialGuaranteeController(() => client, 
                new FinancialGuaranteeDataToDatesMap(), 
                new FinancialGuaranteeDataToDecisionMap(),
                new FinancialGuaranteeDecisionViewModelMap());

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored,
                        A<GetFinancialGuaranteeDataByNotificationApplicationId>.That.Matches(r => r.Id == ReceivedId)))
                .Returns(ReceivedFinancialGuaranteeData);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored,
                        A<GetFinancialGuaranteeDataByNotificationApplicationId>.That.Matches(r => r.Id == CompletedId)))
                .Returns(CompletedFinancialGuaranteeData);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored,
                        A<GetFinancialGuaranteeDataByNotificationApplicationId>.That.Matches(r => r.Id == ApprovedId)))
                .Returns(ApprovedFinancialGuaranteeData);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored,
                        A<GetFinancialGuaranteeDataByNotificationApplicationId>.That.Matches(r => r.Id == AnyGuid)))
                .Returns(new FinancialGuaranteeData
                {
                    Status = FinancialGuaranteeStatus.AwaitingApplication
                });

            model = new FinancialGuaranteeDatesViewModel
            {
                Received = new OptionalDateInputViewModel(AnyDate),
                Completed = new OptionalDateInputViewModel(AnyDate.AddDays(1)),
                Status = "Tony"
            };

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<SetFinancialGuaranteeDates>.Ignored)).Returns(true);
        }

        [Fact]
        public async Task GetDates_ReturnsNewViewModel()
        {
            var result = await controller.Dates(AnyGuid) as ViewResult;

            Assert.IsType<FinancialGuaranteeDatesViewModel>(result.Model);
        }

        [Fact]
        public async Task GetDates_CallsClient()
        {
            var result = await controller.Dates(AnyGuid) as ViewResult;

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored,
                        A<GetFinancialGuaranteeDataByNotificationApplicationId>.That.Matches(r => r.Id == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PostDates_RedirectsToHome()
        {
            var result = await controller.Dates(AnyGuid, model) as RedirectToRouteResult;

            result.AssertControllerReturn("Index", "Home");
        }

        [Fact]
        public async Task PostDates_InvalidViewModel_ReturnsSameView()
        {
            controller.ModelState.AddModelError("Status", "An error occurred");

            var result = await controller.Dates(AnyGuid, model) as ViewResult;

            Assert.Equal(model, result.Model as FinancialGuaranteeDatesViewModel, new ViewModelComparer());
        }

        [Fact]
        public async Task GetDecision_RetrievesFinancialGuaranteeData()
        {
            var result = await controller.Decision(ReceivedId);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored, A<GetFinancialGuaranteeDataByNotificationApplicationId>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetDecision_ReturnsCorrectModel()
        {
            var result = await controller.Decision(CompletedId) as ViewResult;
            
            Assert.IsType<FinancialGuaranteeDecisionViewModel>(result.Model);
        }

        [Fact]
        public async Task GetDecision_GuaranteeInReceivedStatus_ReturnsCorrectModel()
        {
            var result = await controller.Decision(ReceivedId) as ViewResult;

            var model = result.Model as FinancialGuaranteeDecisionViewModel;
            
            Assert.NotNull(model);
            Assert.False(model.IsApplicationCompleted);
        }

        [Fact]
        public async Task GetDecision_GuaranteeInCompletedStatus_RetunsCorrectModel()
        {
            var result = await controller.Decision(CompletedId) as ViewResult;

            var model = result.Model as FinancialGuaranteeDecisionViewModel;

            Assert.True(model.IsApplicationCompleted);
            Assert.Equal(CompletedFinancialGuaranteeData.DecisionRequiredDate, model.DecisionRequiredDate);
            Assert.Equal(CompletedFinancialGuaranteeData.Status, model.Status);
        }

        private class ViewModelComparer : IEqualityComparer<FinancialGuaranteeDatesViewModel>
        {
            public bool Equals(FinancialGuaranteeDatesViewModel x, FinancialGuaranteeDatesViewModel y)
            {
                var comparer = new OptionalDateComparer();

                return x.Status == y.Status
                       && comparer.Equals(x.Completed, y.Completed)
                       && comparer.Equals(x.Received, y.Received);
            }

            public int GetHashCode(FinancialGuaranteeDatesViewModel obj)
            {
                return obj.GetHashCode();
            }
        }

        private class OptionalDateComparer : IEqualityComparer<OptionalDateInputViewModel>
        {
            public bool Equals(OptionalDateInputViewModel x, OptionalDateInputViewModel y)
            {
                return x.Day == y.Day 
                    && x.Month == y.Month 
                    && x.Year == y.Year;
            }

            public int GetHashCode(OptionalDateInputViewModel obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}

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
    using Requests.Admin.FinancialGuarantee;
    using TestHelpers;
    using Xunit;

    public class FinancialGuaranteeControllerTests
    {
        private static readonly Guid AnyGuid = new Guid("3E2AFB2D-03C8-4EF0-878A-FC90A63B32A6");
        private static readonly DateTime AnyDate = new DateTime(2015, 5, 5);
        private static readonly FinancialGuaranteeData ReceivedFinancialGuaranteeData = new FinancialGuaranteeData
                {
                    CompletedDate = null,
                    DecisionRequiredDate = null,
                    ReceivedDate = AnyDate,
                    Status = FinancialGuaranteeStatus.ApplicationReceived
                };

        private readonly FinancialGuaranteeController controller;
        private readonly IIwsClient client;
        private readonly FinancialGuaranteeInformationViewModel model;

        public FinancialGuaranteeControllerTests()
        {
            client = A.Fake<IIwsClient>();

            controller = new FinancialGuaranteeController(() => client);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>.Ignored, A<GetFinancialGuaranteeDataByNotificationApplicationId>.Ignored))
                .Returns(ReceivedFinancialGuaranteeData);

            model = new FinancialGuaranteeInformationViewModel
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

            Assert.IsType<FinancialGuaranteeInformationViewModel>(result.Model);
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

            Assert.Equal(model, result.Model as FinancialGuaranteeInformationViewModel, new ViewModelComparer());
        }

        private class ViewModelComparer : IEqualityComparer<FinancialGuaranteeInformationViewModel>
        {
            public bool Equals(FinancialGuaranteeInformationViewModel x, FinancialGuaranteeInformationViewModel y)
            {
                var comparer = new OptionalDateComparer();

                return x.Status == y.Status
                       && comparer.Equals(x.Completed, y.Completed)
                       && comparer.Equals(x.Received, y.Received);
            }

            public int GetHashCode(FinancialGuaranteeInformationViewModel obj)
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

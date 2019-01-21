namespace EA.Iws.Web.Tests.Unit.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.NotificationApplication.Controllers;
    using EA.Iws.Core.Notification.Audit;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.UpdateHistory;
    using EA.Iws.Web.Areas.NotificationApplication.Views.UpdateHistory;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Xunit;

    public class UpdateHistoryViewModelTests
    {
        public UpdateHistoryViewModel CreateViewModel()
        {
            return new UpdateHistoryViewModel()
            {
                UpdateHistoryItems = A.Fake<List<NotificationAuditForDisplay>>(),
                NotificationId = Guid.NewGuid(),
                HasHistoryItems = true,
                Screens = CreateFakeScreens(new List<NotificationAuditScreen>()),
                PageSize = 10,
                SelectedScreen = "date"
            };
        }

        private List<NotificationAuditScreen> CreateFakeScreens(List<NotificationAuditScreen> list)
        {
            for (int i = 0; i < 10; i++)
            {
                NotificationAuditScreen screen = new NotificationAuditScreen();
                screen.Id = i;
                screen.ScreenName = Guid.NewGuid().ToString();
                list.Add(screen);
            }

            return list;
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void AddCorrectDateFilter_ValidationSuccess()
        {
            UpdateHistoryViewModel vm = this.CreateViewModel();

            vm.StartDay = 1;
            vm.StartMonth = 1;
            vm.StartYear = 2019;
            vm.EndDay = 2;
            vm.EndMonth = 1;
            vm.EndYear = 2019;

            Assert.Equal(true, ValidateModel(vm).Count == 0);
        }

        [Fact]
        public void AddIncorrectDatesFilter_ValidationFailure()
        {
            UpdateHistoryViewModel vm = this.CreateViewModel();

            vm.StartDay = 50;
            vm.StartMonth = 50;
            vm.StartYear = 19;
            vm.EndDay = 60;
            vm.EndMonth = 60;
            vm.EndYear = 19;

            var result = ValidateModel(vm);

            Assert.Equal(true, result.Count > 0);

            Assert.Collection(result,
                item => Assert.Contains(IndexResources.DayError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.MonthError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.YearError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.DayError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.MonthError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.YearError, item.ErrorMessage));
        }

        [Fact]
        public void AddEmptyDates_ValidationFailure()
        {
            UpdateHistoryViewModel vm = this.CreateViewModel();

            vm.StartDay = null;
            vm.StartMonth = null;
            vm.StartYear = null;
            vm.EndDay = null;
            vm.EndMonth = null;
            vm.EndYear = null;

            var result = ValidateModel(vm);

            Assert.Equal(true, result.Count > 0);

            Assert.Collection(result,
                item => Assert.Contains(IndexResources.DayError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.MonthError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.YearError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.DayError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.MonthError, item.ErrorMessage),
                item => Assert.Contains(IndexResources.YearError, item.ErrorMessage));
        }

        [Fact]
        public void StartDateIsAfterEndDate_ValidationError()
        {
            UpdateHistoryViewModel vm = this.CreateViewModel();

            vm.SetDates(DateTime.Now, DateTime.Now.AddDays(-1));

            var result = ValidateModel(vm);

            Assert.Equal(true, result.Count > 0);

            Assert.Collection(result, item => Assert.Contains(IndexResources.FromDateAfterToDate, item.ErrorMessage));
        }

        [Fact]
        public void CheckFilterList()
        {
            UpdateHistoryViewModel vm = this.CreateViewModel();

            foreach (var selectListItem in vm.FilterTerms.Where(p => p.Text != "Date" && p.Text != "View all"))
            {
                Assert.Equal(true, vm.Screens.Count(p => p.ScreenName == selectListItem.Text) > 0);
                Assert.Equal(true, vm.Screens.Count(p => p.Id == int.Parse(selectListItem.Value)) > 0);
            }

            Assert.Equal(true, vm.FilterTerms.Count(p => p.Text == "View all") > 0);
            Assert.Equal(true, vm.FilterTerms.Count(p => p.Text == "Date") > 0);
            Assert.Equal(true, vm.FilterTerms.Count(p => p.Value == string.Empty) == 1);
            Assert.Equal(true, vm.FilterTerms.Count(p => p.Value == "date") == 1);
        }
    }
}

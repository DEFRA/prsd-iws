namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Helpers;
    using Mappings;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetFinancialGuaranteeDataByNotificationApplicationIdHandlerTests
    {
        private static readonly Guid PendingId = new Guid("19D5FA09-8576-46E9-944E-3FAF84D56685");
        private static readonly Guid ReceivedId = new Guid("8895ACDA-7788-4621-A9F1-5F4A0A7C5309");
        private static readonly Guid CompletedId = new Guid("5455827A-A555-4949-80CD-90680C2DEEB4");
        private static readonly DateTime AnyDate = new DateTime(2015, 3, 25);

        private readonly GetFinancialGuaranteeDataByNotificationApplicationIdHandler handler;

        public GetFinancialGuaranteeDataByNotificationApplicationIdHandlerTests()
        {
            var context = A.Fake<IwsContext>();

            A.CallTo(() => context.NotificationAssessments).Returns(GenerateFinancialGuaranteesDbSet());

            handler = new GetFinancialGuaranteeDataByNotificationApplicationIdHandler(context, new FinancialGuaranteeMap());
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new GetFinancialGuaranteeDataByNotificationApplicationId(Guid.Empty)));
        }

        [Fact]
        public async Task NotificationPending_ReturnsCorrectObject()
        {
            var result = await handler.HandleAsync(new GetFinancialGuaranteeDataByNotificationApplicationId(PendingId));

            Assert.Equal(FinancialGuaranteeStatus.AwaitingApplication, result.Status);
        }

        public DbSet<NotificationAssessment> GenerateFinancialGuaranteesDbSet()
        {
            var helper = new DbContextHelper();

            return helper.GetAsyncEnabledDbSet(new[]
            {
                CreateAssessmentWithFinancialGuarantee(PendingId, FinancialGuaranteeStatus.AwaitingApplication,
                    null, null),
                CreateAssessmentWithFinancialGuarantee(ReceivedId, FinancialGuaranteeStatus.ApplicationReceived, AnyDate, null),
                CreateAssessmentWithFinancialGuarantee(CompletedId, FinancialGuaranteeStatus.ApplicationComplete, AnyDate,
                    AnyDate.AddDays(1))
            });
        }

        public NotificationAssessment CreateAssessmentWithFinancialGuarantee(Guid notificationId, FinancialGuaranteeStatus status, DateTime? receivedDate, DateTime? completedDate)
        {
            var assessment = new NotificationAssessment(notificationId);
            var financialGuarantee = FinancialGuarantee.Create();

            if (receivedDate.HasValue)
            {
                ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, receivedDate.Value, financialGuarantee);
            }

            if (completedDate.HasValue)
            {
                ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, completedDate.Value, financialGuarantee);
            }

            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, status, financialGuarantee);
            ObjectInstantiator<NotificationAssessment>.SetProperty(na => na.FinancialGuarantee, financialGuarantee, assessment);

            return assessment;
        }
    }
}

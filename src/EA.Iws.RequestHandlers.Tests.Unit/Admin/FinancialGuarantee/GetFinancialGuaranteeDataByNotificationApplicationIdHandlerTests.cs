namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using RequestHandlers.Admin.FinancialGuarantee;
    using RequestHandlers.Mappings;
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
            var context = new TestIwsContext();

            context.FinancialGuarantees.AddRange(GenerateFinancialGuaranteesDbSet());
            context.NotificationApplications.AddRange(GenerateApplications());

            handler = new GetFinancialGuaranteeDataByNotificationApplicationIdHandler(context, new FinancialGuaranteeMap(new WorkingDayCalculator(context)));
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

        public IEnumerable<FinancialGuarantee> GenerateFinancialGuaranteesDbSet()
        {
            return new[]
            {
                CreateFinancialGuarantee(PendingId, FinancialGuaranteeStatus.AwaitingApplication,
                    null, null),
                CreateFinancialGuarantee(ReceivedId, FinancialGuaranteeStatus.ApplicationReceived, AnyDate, null),
                CreateFinancialGuarantee(CompletedId, FinancialGuaranteeStatus.ApplicationComplete, AnyDate,
                    AnyDate.AddDays(1))
            };
        }

        public IEnumerable<NotificationApplication> GenerateApplications()
        {
            return new[]
            {
                NotificationApplicationFactory.Create(PendingId),
                NotificationApplicationFactory.Create(ReceivedId),
                NotificationApplicationFactory.Create(CompletedId)
            };
        } 

        public FinancialGuarantee CreateFinancialGuarantee(Guid notificationId, FinancialGuaranteeStatus status, DateTime? receivedDate, DateTime? completedDate)
        {
            var financialGuarantee = FinancialGuarantee.Create(notificationId);

            if (receivedDate.HasValue)
            {
                ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, receivedDate.Value, financialGuarantee);
            }

            if (completedDate.HasValue)
            {
                ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, completedDate.Value, financialGuarantee);
            }

            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, status, financialGuarantee);

            return financialGuarantee;
        }
    }
}

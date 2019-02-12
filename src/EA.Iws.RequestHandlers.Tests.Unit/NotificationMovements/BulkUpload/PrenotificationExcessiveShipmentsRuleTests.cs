namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class PrenotificationExcessiveShipmentsRuleTests
    {
        private readonly INotificationMovementsSummaryRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");
        private readonly IMovementRepository movementRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private const int MaxActiveLoads = 5;
        private const int CurrentActiveLoads = 3;

        private PrenotificationExcessiveShipmentsRule rule;

        public PrenotificationExcessiveShipmentsRuleTests()
        {
            movementRepository = A.Fake<IMovementRepository>();
            financialGuaranteeRepository = A.Fake<IFinancialGuaranteeRepository>();

            var testCollection = new TestableFinancialGuaranteeCollection(notificationId)
            {
                FinancialGuarantees = new List<TestableFinancialGuarantee>()
                {
                    new TestableFinancialGuarantee()
                    {
                        ActiveLoadsPermitted = MaxActiveLoads,
                        Status = FinancialGuaranteeStatus.Approved
                    }
                }
            };

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(notificationId)).Returns(testCollection);
            A.CallTo(() => movementRepository.GetActiveMovements(notificationId))
                .Returns(A.CollectionOfFake<Movement>(CurrentActiveLoads));

            rule = new PrenotificationExcessiveShipmentsRule(movementRepository, financialGuaranteeRepository);
        }

        [Fact]
        public async Task ShipmentsGroupedByDate_MoreThanActiveLoads_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                 new PrenotificationMovement()
                {
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = new DateTime(2019, 2, 2)
                },
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(PrenotificationContentRules.ActiveLoadsGrouped, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }
    }
}

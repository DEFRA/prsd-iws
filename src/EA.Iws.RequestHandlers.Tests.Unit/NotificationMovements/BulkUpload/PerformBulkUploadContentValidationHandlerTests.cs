namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Core.Movement.BulkPrenotification;
    using Core.PackagingType;
    using Core.Rules;
    using Core.Shared;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.Movement.BulkUpload;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Requests.NotificationMovements.BulkUpload;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class PerformBulkUploadContentValidationHandlerTests
    {
        private readonly PerformPrenotificationContentValidationHandler handler;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;
        private readonly IDraftMovementRepository repository;
        private readonly IPrenotificationContentRule contentRule;
        private readonly IMovementRepository movementRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly Guid notificationId;
        private const int MaxShipments = 50;
        private const int MaxActiveLoads = 3;
        private const int DateGroups = 2;

        public PerformBulkUploadContentValidationHandlerTests()
        {
            notificationId = Guid.NewGuid();

            mapper = A.Fake<IMap<DataTable, List<PrenotificationMovement>>>();
            repository = A.Fake<IDraftMovementRepository>();
            contentRule = A.Fake<IPrenotificationContentRule>();
            movementRepository = A.Fake<IMovementRepository>();
            financialGuaranteeRepository = A.Fake<IFinancialGuaranteeRepository>();

            var contentRules = new List<IPrenotificationContentRule>()
            {
                contentRule
            };

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

            var testFutureActiveMovements = new List<TestableMovement>()
            {
                new TestableMovement()
                {
                    Status = MovementStatus.Submitted,
                    Date = new DateTime(2019, 1, 1)
                },
                new TestableMovement()
                {
                    Status = MovementStatus.Submitted,
                    Date = new DateTime(2019, 1, 1)
                },
                new TestableMovement()
                {
                    Status = MovementStatus.Submitted,
                    Date = new DateTime(2019, 1, 15)
                },
                new TestableMovement()
                {
                    Status = MovementStatus.Submitted,
                    Date = new DateTime(2019, 1, 15)
                }
            };

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(notificationId)).Returns(testCollection);
            A.CallTo(() => movementRepository.GetFutureActiveMovements(notificationId))
                .Returns(testFutureActiveMovements);

            handler = new PerformPrenotificationContentValidationHandler(contentRules, mapper, repository, movementRepository, financialGuaranteeRepository);
        }

        [Fact]
        public async Task ExceedsMaxRows_ContentRulesFailed()
        {
            var summary = new PrenotificationRulesSummary();
            var message = new PerformPrenotificationContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored))
                .Returns(A.CollectionOfFake<PrenotificationMovement>(MaxShipments + 1).ToList());

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingShipmentNumber_ContentRulesFailed()
        {
            var summary = new PrenotificationRulesSummary();
            var message = new PerformPrenotificationContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = null,
                    MissingShipmentNumber = true
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingNotificationNumber_ContentRulesFailed()
        {
            var summary = new PrenotificationRulesSummary();
            var message = new PerformPrenotificationContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    MissingNotificationNumber = true
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingData_ContentRulesFailed()
        {
            var summary = new PrenotificationRulesSummary();
            var message = new PerformPrenotificationContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 1,
                    MissingQuantity = true,
                    MissingDateOfShipment = true
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task ContentRulesFailed_DoesNotSaveToDraft()
        {
            var summary = new PrenotificationRulesSummary();
            var message = new PerformPrenotificationContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(A.CollectionOfFake<PrenotificationMovement>(5).ToList());
            A.CallTo(() => contentRule.GetResult(A<List<PrenotificationMovement>>.Ignored, notificationId))
                .Returns(new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MissingData,
                    MessageLevel.Error, "Missing data", 0));

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
            A.CallTo(() => repository.AddPrenotifications(A<Guid>.Ignored, A<List<PrenotificationMovement>>.Ignored, "Test")).MustNotHaveHappened();
        }

        [Fact]
        public async Task ContentRulesSuccess_SavesToDraft()
        {
            var summary = new PrenotificationRulesSummary();
            var message = new PerformPrenotificationContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 123456",
                    ShipmentNumber = 1,
                    Quantity = 1m,
                    Unit = ShipmentQuantityUnits.Tonnes,
                    PackagingTypes = new List<PackagingType>() { PackagingType.Drum, PackagingType.Bag },
                    ActualDateOfShipment = SystemTime.UtcNow
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);
            A.CallTo(() => contentRule.GetResult(A<List<PrenotificationMovement>>.Ignored, notificationId))
                .Returns(new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MissingData,
                    MessageLevel.Success, "Test", 0));

            var response = await handler.HandleAsync(message);

            Assert.True(response.IsContentRulesSuccess);
            A.CallTo(() => repository.AddPrenotifications(A<Guid>.Ignored, A<List<PrenotificationMovement>>.Ignored, "Test"))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task NewShipmentsGroupedByDate_MoreThanActiveLoads_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 2,
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 3,
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 4,
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 5,
                    ActualDateOfShipment = new DateTime(2019, 1, 15)
                },
                 new PrenotificationMovement()
                {
                     ShipmentNumber = 6,
                    ActualDateOfShipment = new DateTime(2019, 1, 15)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 7,
                    ActualDateOfShipment = new DateTime(2019, 1, 15)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 8,
                    ActualDateOfShipment = new DateTime(2019, 1, 15)
                }
            };

            var rules = (await handler.GetActiveLoadsRule(movements, notificationId)).ToList();

            Assert.True(rules.Any());
            Assert.True(rules.Count == DateGroups);
            Assert.True(rules.All(r => r.Rule == PrenotificationContentRules.ActiveLoadsDataShipments));
            Assert.True(rules.All(r => r.MessageLevel == MessageLevel.Error));
        }

        [Fact]
        public async Task NewAndExistingShipmentsGroupedByDate_MoreThanActiveLoads_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 2,
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 3,
                    ActualDateOfShipment = new DateTime(2019, 1, 15)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 4,
                    ActualDateOfShipment = new DateTime(2019, 1, 15)
                }
            };

            var rules = (await handler.GetActiveLoadsRule(movements, notificationId)).ToList();

            Assert.True(rules.Any());
            Assert.True(rules.Count == DateGroups);
            Assert.True(rules.All(r => r.Rule == PrenotificationContentRules.ActiveLoadsWithExistingShipments));
            Assert.True(rules.All(r => r.MessageLevel == MessageLevel.Error));
        }

        [Fact]
        public async Task NewAndExistingShipmentsGroupedByDate_LessThanEqualActiveLoads_Success()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    ActualDateOfShipment = new DateTime(2019, 1, 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 2,
                    ActualDateOfShipment = new DateTime(2019, 1, 15)
                }
            };

            var rules = (await handler.GetActiveLoadsRule(movements, notificationId)).ToList();

            Assert.False(rules.Any());
        }
    }
}

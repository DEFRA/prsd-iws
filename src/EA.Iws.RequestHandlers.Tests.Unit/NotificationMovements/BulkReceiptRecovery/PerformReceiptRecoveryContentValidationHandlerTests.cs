namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Requests.NotificationMovements.BulkUpload;
    using Xunit;

    public class PerformReceiptRecoveryContentValidationHandlerTests
    {
        private readonly PerformReceiptRecoveryContentValidationHandler handler;
        private readonly IEnumerable<IReceiptRecoveryContentRule> contentRules;
        private readonly IMap<DataTable, List<ReceiptRecoveryMovement>> mapper;
        private readonly IReceiptRecoveryContentRule contentRule;
        private const int MaxShipments = 50;

        public PerformReceiptRecoveryContentValidationHandlerTests()
        {
            mapper = A.Fake<IMap<DataTable, List<ReceiptRecoveryMovement>>>();
            contentRule = A.Fake<IReceiptRecoveryContentRule>();

            contentRules = new List<IReceiptRecoveryContentRule>(1)
            {
                contentRule
            };

            handler = new PerformReceiptRecoveryContentValidationHandler(contentRules, mapper);
        }

        [Fact]
        public async Task ExceedsMaxRows_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored))
                .Returns(A.CollectionOfFake<ReceiptRecoveryMovement>(MaxShipments + 1).ToList());

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Fact]
        public async Task MissingShipmentNumber_ContentRulesFailed()
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
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
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    MissingNotificationNumber = true
                }
            };

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Theory]
        [MemberData("MissingReceiptData")]
        public async Task MissingReceiptData_ContentRulesFailed(List<ReceiptRecoveryMovement> movements)
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);

            Assert.False(response.IsContentRulesSuccess);
        }

        [Theory]
        [MemberData("CorrectReceiptData")]
        public async Task MissingReceiptData_ContentRulesSuccess(List<ReceiptRecoveryMovement> movements)
        {
            var notificationId = Guid.NewGuid();
            var summary = new ReceiptRecoveryRulesSummary();
            var message = new PerformReceiptRecoveryContentValidation(summary, notificationId, new DataTable(), "Test", false);

            A.CallTo(() => mapper.Map(A<DataTable>.Ignored)).Returns(movements);

            var response = await handler.HandleAsync(message);
            
            Assert.True(response.ContentRulesResults
                .Where(r => r.Rule == ReceiptRecoveryContentRules.MissingReceiptData)
                .Single().MessageLevel == Core.Rules.MessageLevel.Success);
        }

        public static IEnumerable<object[]> MissingReceiptData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new List<ReceiptRecoveryMovement>()
                        {
                            new ReceiptRecoveryMovement()
                            {
                                NotificationNumber = "GB 0001 123456",
                                ShipmentNumber = 1,
                                MissingReceivedDate = true,
                                MissingQuantity = false,
                                MissingUnits = false
                            }
                        }
                    },
                    new object[] 
                    {
                        new List<ReceiptRecoveryMovement>()
                        {
                            new ReceiptRecoveryMovement()
                            {
                                NotificationNumber = "GB 0001 123456",
                                ShipmentNumber = 2,
                                MissingReceivedDate = false,
                                MissingQuantity = true,
                                MissingUnits = false
                            }
                        }
                    },
                    new object[]
                    {
                        new List<ReceiptRecoveryMovement>()
                        {
                            new ReceiptRecoveryMovement()
                            {
                                NotificationNumber = "GB 0001 123456",
                                ShipmentNumber = 3,
                                MissingReceivedDate = false,
                                MissingQuantity = false,
                                MissingUnits = true
                            }
                        }
                    }
                };
            }
        }

        public static IEnumerable<object[]> CorrectReceiptData
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new List<ReceiptRecoveryMovement>()
                        {
                            new ReceiptRecoveryMovement()
                            {
                                NotificationNumber = "GB 0001 123456",
                                ShipmentNumber = 1,
                                MissingReceivedDate = false,
                                MissingQuantity = false,
                                MissingUnits = false
                            }
                        }
                    }
                };
            }
        }
    }
}

namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Movement.BulkReceiptRecovery;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;

    public class PerformReceiptRecoveryContentValidationHandlerTests
    {
        private readonly PerformReceiptRecoveryContentValidationHandler handler;
        private readonly IEnumerable<IReceiptRecoveryContentRule> contentRules;
        private readonly IMap<DataTable, List<ReceiptRecoveryMovement>> mapper;
        private readonly IReceiptRecoveryContentRule contentRule;

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
    }
}

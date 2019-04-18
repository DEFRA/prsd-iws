namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using Xunit;

    public class GetMovementAuditByNotificationIdHandlerTests
    {
        private readonly GetMovementAuditByNotificationIdHandler handler;
        private readonly IMapper mapper;
        private readonly IMovementAuditRepository repository;

        private readonly Guid notificationId;
        private const int AuditCount = 10;
        private const int PageSize = 15;
        private const string AnyString = "test";

        public GetMovementAuditByNotificationIdHandlerTests()
        {
            notificationId = Guid.NewGuid();

            mapper = A.Fake<IMapper>();
            repository = A.Fake<IMovementAuditRepository>();

            A.CallTo(() => repository.GetTotalNumberOfShipmentAudits(notificationId)).Returns(AuditCount);

            handler = new GetMovementAuditByNotificationIdHandler(mapper, repository);
        }

        [Fact]
        public async Task GetMovementAuditByNotificationIdHandler_GetPagedShipmentAuditsById()
        {
            var request = GetRequest(1, 5);

            await handler.HandleAsync(request);

            A.CallTo(() => repository.GetPagedShipmentAuditsById(notificationId, 1, PageSize, request.ShipmentNumber))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetMovementAuditByNotificationIdHandler_MapsToShipmentAudit()
        {
            var request = GetRequest(1, 5);

            await handler.HandleAsync(request);

            A.CallTo(
                    () => mapper.Map<IEnumerable<MovementAudit>, ShipmentAuditData>(A<IEnumerable<MovementAudit>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private GetMovementAuditByNotificationId GetRequest(int pageNumber, int? shipmentNumber)
        {
            SetAuditData(pageNumber, shipmentNumber);
            return new GetMovementAuditByNotificationId(notificationId, pageNumber, shipmentNumber);
        }

        private void SetAuditData(int pageNumber, int? shipmentNumber)
        {
            var audits = new List<MovementAudit>();

            var auditTypeValues = Enum.GetValues(typeof(MovementAuditType));
            var random = new Random();

            for (var i = 0; i < AuditCount; i++)
            {
                var number = shipmentNumber.HasValue ? shipmentNumber.Value : i + 1;
                var auditType = (MovementAuditType)auditTypeValues.GetValue(random.Next(auditTypeValues.Length));

                audits.Add(new MovementAudit(notificationId, number, AnyString, (int)auditType,
                    SystemTime.UtcNow));
            }

            SetMapper(audits);

            A.CallTo(() => repository.GetPagedShipmentAuditsById(notificationId, pageNumber, PageSize, shipmentNumber)).Returns(audits);
        }

        private void SetMapper(IEnumerable<MovementAudit> movementAudits)
        {
            var data = new ShipmentAuditData
            {
                TableData = movementAudits
                    .Select(shipmentAudit => mapper.Map<ShipmentAuditRecord>(shipmentAudit))
                    .OrderByDescending(x => x.ShipmentNumber)
                    .ThenByDescending(x => x.DateAdded)
                    .ToList()
            };

            A.CallTo(
                    () => mapper.Map<IEnumerable<MovementAudit>, ShipmentAuditData>(A<IEnumerable<MovementAudit>>.Ignored))
                .Returns(data);
        }
    }
}

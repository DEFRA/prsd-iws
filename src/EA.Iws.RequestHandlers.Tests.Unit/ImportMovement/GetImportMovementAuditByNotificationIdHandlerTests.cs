namespace EA.Iws.RequestHandlers.Tests.Unit.ImportMovement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain.ImportMovement;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using RequestHandlers.ImportMovement;
    using Requests.ImportMovement;
    using Xunit;

    public class GetImportMovementAuditByNotificationIdHandlerTests
    {
        private readonly GetImportMovementAuditByNotificationIdHandler handler;
        private readonly IMapper mapper;
        private readonly IImportMovementAuditRepository repository;

        private readonly Guid notificationId;
        private const int AuditCount = 10;
        private const int PageSize = 15;
        private const string AnyString = "test";

        public GetImportMovementAuditByNotificationIdHandlerTests()
        {
            notificationId = Guid.NewGuid();

            mapper = A.Fake<IMapper>();
            repository = A.Fake<IImportMovementAuditRepository>();

            handler = new GetImportMovementAuditByNotificationIdHandler(mapper, repository);
        }

        [Fact]
        public async Task GetImportMovementAuditByNotificationIdHandler_GetPagedShipmentAuditsById()
        {
            var request = GetRequest(1, 5);

            await handler.HandleAsync(request);

            A.CallTo(() => repository.GetPagedShipmentAuditsById(notificationId, 1, PageSize, request.ShipmentNumber))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetImportMovementAuditByNotificationIdHandle_MapsToShipmentAudit()
        {
            var request = GetRequest(1, 5);

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        mapper.Map<IEnumerable<ImportMovementAudit>, ShipmentAuditData>(
                            A<IEnumerable<ImportMovementAudit>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        private GetImportMovementAuditByNotificationId GetRequest(int pageNumber, int? shipmentNumber)
        {
            SetAuditData(pageNumber, shipmentNumber);
            return new GetImportMovementAuditByNotificationId(notificationId, pageNumber, shipmentNumber);
        }

        private void SetAuditData(int pageNumber, int? shipmentNumber)
        {
            var audits = new List<ImportMovementAudit>();

            var auditTypeValues = Enum.GetValues(typeof(MovementAuditType));
            var random = new Random();

            for (var i = 0; i < AuditCount; i++)
            {
                var number = shipmentNumber.HasValue ? shipmentNumber.Value : i + 1;
                var auditType = (MovementAuditType)auditTypeValues.GetValue(random.Next(auditTypeValues.Length));

                audits.Add(new ImportMovementAudit(notificationId, number, AnyString, (int)auditType,
                    SystemTime.UtcNow));
            }

            SetMapper(audits);

            A.CallTo(() => repository.GetPagedShipmentAuditsById(notificationId, pageNumber, PageSize, shipmentNumber)).Returns(audits);
        }

        private void SetMapper(IEnumerable<ImportMovementAudit> movementAudits)
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
                    () => mapper.Map<IEnumerable<ImportMovementAudit>, ShipmentAuditData>(A<IEnumerable<ImportMovementAudit>>.Ignored))
                .Returns(data);
        }
    }
}
